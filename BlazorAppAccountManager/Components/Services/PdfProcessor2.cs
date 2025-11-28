using SkiaSharp;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using Tesseract;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Rendering.Skia;

namespace BlazorAppAccountManager.Components.Services
{
    public class PdfProcessor2
    {
        private readonly string _tessDataPath;

        public PdfProcessor2(string tessDataPath)
        {
            _tessDataPath = tessDataPath ?? throw new ArgumentNullException(nameof(tessDataPath));
            if (!Directory.Exists(_tessDataPath))
            {
                throw new DirectoryNotFoundException($"Tesseract tessdata directory not found at: {_tessDataPath}");
            }
        }

        /// <summary>
        /// 从 PDF 中提取所有文本 (优先尝试原生文本提取，如果为空则使用 OCR)
        /// </summary>
        public string ProcessPdf(MemoryStream pdfStream)
        {
            if (pdfStream == null || pdfStream.Length == 0)
                throw new ArgumentException("PDF stream cannot be null or empty.", nameof(pdfStream));

            // 确保流位置在开始处，因为会多次读取
            pdfStream.Position = 0;

            string nativeText = ExtractTextFromNativePdf(pdfStream);

            if (!string.IsNullOrWhiteSpace(nativeText))
            {
                Console.WriteLine("Detected: Native text PDF. Extracting text directly.");
                // 对于原生文本，可能也需要进行一些清理
                return CleanText(nativeText);
            }
            else
            {
                Console.WriteLine("Detected: Scanned/Image PDF or native text extraction failed. Extracting text using OCR...");
                // 确保流位置在开始处，以便 OCR 渲染
                pdfStream.Position = 0;
                string ocrText = ExtractTextFromScannedPdf2(pdfStream);
                Console.WriteLine($"Raw OCR Text: {ocrText}");
                string cleanedOcrText = CleanText(ocrText);
                Console.WriteLine($"Cleaned OCR Text: {cleanedOcrText}");
                return cleanedOcrText;
            }
        }

        private static byte[] ConvertToPng(byte[] rawImageBytes)
        {
            using (var ms = new MemoryStream(rawImageBytes))
            {
                // 读取原始图片（自动识别格式）
                using (var image = Image.FromStream(ms))
                {
                    // 转换为 PNG 格式并输出字节数组
                    using (var pngMs = new MemoryStream())
                    {
                        image.Save(pngMs, System.Drawing.Imaging.ImageFormat.Png);
                        return pngMs.ToArray(); // 返回标准 PNG 格式的字节
                    }
                }
            }
        }

        // 对单张图片进行 OCR 识别
        private string OcrImage(byte[] imageBytes)
        {
            // 临时保存图片（Tesseract 需读取图片文件或流）
            var tempPath = Path.GetTempFileName() + ".png";
            File.WriteAllBytes(tempPath, ConvertToPng(imageBytes));

            try
            {
                // 初始化 Tesseract（指定语言包目录和语言）
                using (var engine = new TesseractEngine(_tessDataPath, "chi_sim+eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(tempPath))
                    {
                        using (var page = engine.Process(img))
                        {
                            return page.GetText(); // 返回识别的文字
                        }
                    }
                }
            }
            finally
            {
                File.Delete(tempPath); // 清理临时文件
            }
        }

        Pix ConvertSkBitmapToPix(SKBitmap skBitmap)
        {
            // 将 SKBitmap 编码为 PNG 字节流（Tesseract 支持 PNG 格式）
            using (var image = SKImage.FromBitmap(skBitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100)) // 100 是质量（无损）
            using (var stream = new MemoryStream(data.ToArray()))
            {
                // 从内存流加载为 Pix
                return Pix.LoadFromMemory(stream.ToArray());
            }
        }

        string ExtractTextFromScannedPdf2(MemoryStream pdfStream)
        {
            var textBuilder = new StringBuilder();
            try
            {
                // UglyToad.PdfPig 需要一个可读写的流，并且不改变其 Position
                // 所以我们复制一份流以避免干扰原始流的位置
                using (var tempStream = new MemoryStream(pdfStream.ToArray()))
                {
                    using (var document = UglyToad.PdfPig.PdfDocument.Open(tempStream, SkiaRenderingParsingOptions.Instance))
                    {
                        if (document.NumberOfPages == 0)
                        {
                            return "PDF 文件无页面";
                        }

                        document.AddSkiaPageFactory();

                        using (var engine = new TesseractEngine(_tessDataPath, "chi_sim+eng", EngineMode.Default))
                        {
                            for (int p = 1; p <= document.NumberOfPages; p++)
                            {

                                var scale = 2.0f; // 放大倍数，提升识别率

                                var bitmap = document.GetPageAsSKBitmap(p, scale, RGBColor.White);

                                // 4. 将 SKBitmap 转换为 Tesseract 可识别的 Pix 对象
                                using (var pix = ConvertSkBitmapToPix(bitmap))
                                {
                                    // 5. 调用 Tesseract 识别文字
                                    using (var pageResult = engine.Process(pix))
                                    {
                                        string text = pageResult.GetText();
                                        textBuilder.Append(text);
                                    }
                                }
                            }
                        }

                            
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting text from native PDF (PdfPig): {ex.Message}");
                // 如果是原生文本提取失败，可能是加密或其他非图片问题，我们返回空字符串让 OCR 接管
                return string.Empty;
            }
            return textBuilder.ToString();
        }       

        /// <summary>
        /// 从原生 PDF 中提取所有文本 (使用 UglyToad.PdfPig)
        /// </summary>
        private string ExtractTextFromNativePdf(MemoryStream pdfStream)
        {
            var textBuilder = new StringBuilder();
            try
            {
                // UglyToad.PdfPig 需要一个可读写的流，并且不改变其 Position
                // 所以我们复制一份流以避免干扰原始流的位置
                using (var tempStream = new MemoryStream(pdfStream.ToArray()))
                {
                    using (var document = UglyToad.PdfPig.PdfDocument.Open(tempStream))
                    {
                        foreach (var page in document.GetPages())
                        {
                            var pageText = page.Text;
                            if (!string.IsNullOrWhiteSpace(pageText))
                            {
                                textBuilder.Append(pageText);
                                // 添加换行符以保持页面间的文本分离，否则所有文本会挤在一起
                                textBuilder.Append(Environment.NewLine);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting text from native PDF (PdfPig): {ex.Message}");
                // 如果是原生文本提取失败，可能是加密或其他非图片问题，我们返回空字符串让 OCR 接管
                return string.Empty;
            }
            return textBuilder.ToString();
        }
        /// <summary>
        /// 清理提取到的文本：去除空格、控制字符和重复标点
        /// </summary>
        private string CleanText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // 1. 去除所有空白字符（包括普通空格、不间断空格、制表符、换行符等）
            // 注意：如果想保留段落或单词间的单一空格，可以调整这个正则
            // 例如，保留单词间单个空格：text = Regex.Replace(text, @"\s+", " ").Trim();
            text = Regex.Replace(text, @"\s+", "");

            // 2. 去除控制字符（如 ASCII 0-31 和 127）
            text = Regex.Replace(text, @"[\x00-\x1F\x7F]", "");

            // 3. 去除重复的标点符号（根据需求调整，这里示例部分常用标点）
            // 例如：!!! -> ! ,,, -> , 。。。 -> 。
            text = Regex.Replace(text, @"([.,;()（），。；：:！!？?])\1+", "$1");

            // 4. 去除首尾空白（虽然前面已经去除了所有\s+，但Trim还是一个好的习惯，以防万一）
            text = text.Trim();

            return text;
        }
    }
}
