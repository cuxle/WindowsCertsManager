using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using Tesseract;

namespace BlazorAppAccountManager.Components.Services
{
    public class PdfProcessor
    {
        private string _tessDataPath;

        public PdfProcessor(string tessDataPath)
        {
            // Tesseract 的语言数据目录
            _tessDataPath = tessDataPath;
        }

        /// <summary>
        /// 判断 PDF 是否为原生文本类型
        /// </summary>
        public bool IsNativePdf(MemoryStream pdfStream)
        {
            try
            {
                using (var pdfDocument = new iText.Kernel.Pdf.PdfDocument(new PdfReader(pdfStream)))
                {
                    var page = pdfDocument.GetPage(1); // 尝试读取第一页
                    var text = PdfTextExtractor.GetTextFromPage(page);
                    return !string.IsNullOrWhiteSpace(text);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking PDF type: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 从原生 PDF 中提取所有文本
        /// </summary>
        public string ExtractTextFromNativePdf(MemoryStream pdfStream)
        {
            var fullText = new System.Text.StringBuilder();
            try
            {
                using (var pdfDocument = new iText.Kernel.Pdf.PdfDocument(new PdfReader(pdfStream)))
                {
                    for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                    {
                        var page = pdfDocument.GetPage(i);
                        var text = PdfTextExtractor.GetTextFromPage(page);
                        fullText.Append(text);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting text from native PDF: {ex.Message}");
            }
            return fullText.ToString();
        }

        /// <summary>
        /// 使用 OCR 从图片型 PDF 中提取所有文本
        /// </summary>
        public string ExtractTextFromScannedPdf(MemoryStream pdfStream)
        {
            var fullText = new System.Text.StringBuilder();
            PdfCommon.Initialize();
            try
            {
                // Initialize Pdfium.Net library for rendering
                using (var doc = Patagames.Pdf.Net.PdfDocument.Load(pdfStream))
                {
                    List<Bitmap> bitmaps = new();
                    foreach (var page in doc.Pages)
                    {
                        foreach (var obj in page.PageObjects)
                        {
                            var imageObject = obj as PdfImageObject;
                            if (imageObject == null)
                                continue; //if not an image object then nothing do
                            var pdfBitmap = imageObject.Bitmap;

                            var bitmap = ToBitmap(pdfBitmap);

                            bitmaps.Add(bitmap);
                        }
                    }

                    using (var engine = new TesseractEngine(_tessDataPath, "chi_sim+eng", EngineMode.Default))
                    {
                        foreach (var bitmap in bitmaps)
                        {
                            // 修正：使用内存流将Bitmap转换为Pix
                            using (var memoryStream = new MemoryStream())
                            {
                                // 将Bitmap保存到内存流（PNG格式保留透明度和质量）
                                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                memoryStream.Position = 0;

                                // 从内存流加载为Pix对象
                                using (var img = Pix.LoadFromMemory(memoryStream.ToArray()))
                                using (var page = engine.Process(img, PageSegMode.Auto))
                                {
                                    fullText.Append(page.GetText());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting text with OCR: {ex.Message}");
            }

            return fullText.ToString();
        }

        private PixelFormat GetPixelFormat(BitmapFormats pdfFormat)
        {
            return pdfFormat switch
            {
                // 1位掩码格式
                BitmapFormats.FXDIB_1bppMask => PixelFormat.Format1bppIndexed,
                // 1位RGB格式
                BitmapFormats.FXDIB_1bppRgb => PixelFormat.Format1bppIndexed,
                // 1位CMYK格式（转为32位ARGB处理）
                BitmapFormats.FXDIB_1bppCmyk => PixelFormat.Format32bppArgb,
                // 8位掩码格式
                BitmapFormats.FXDIB_8bppMask => PixelFormat.Format8bppIndexed,
                // 8位RGB格式（索引色）
                BitmapFormats.FXDIB_8bppRgb => PixelFormat.Format8bppIndexed,
                // 8位带Alpha的RGB格式
                BitmapFormats.FXDIB_8bppRgba => PixelFormat.Format32bppArgb,
                // 8位CMYK格式（转为32位ARGB处理）
                BitmapFormats.FXDIB_8bppCmyk => PixelFormat.Format32bppArgb,
                // 8位带Alpha的CMYK格式（转为32位ARGB处理）
                BitmapFormats.FXDIB_8bppCmyka => PixelFormat.Format32bppArgb,
                // 24位RGB格式
                BitmapFormats.FXDIB_Rgb => PixelFormat.Format24bppRgb,
                // 带Alpha的RGB格式
                BitmapFormats.FXDIB_Rgba => PixelFormat.Format32bppArgb,
                // 32位RGB格式
                BitmapFormats.FXDIB_Rgb32 => PixelFormat.Format32bppRgb,
                // ARGB格式
                BitmapFormats.FXDIB_Argb => PixelFormat.Format32bppArgb,
                // CMYK格式（转为32位ARGB处理）
                BitmapFormats.FXDIB_Cmyk => PixelFormat.Format32bppArgb,
                // 带Alpha的CMYK格式（转为32位ARGB处理）
                BitmapFormats.FXDIB_Cmyka => PixelFormat.Format32bppArgb,
                // 无效格式
                BitmapFormats.FXDIB_Invalid => throw new ArgumentException("无效的像素格式"),
                _ => throw new NotSupportedException($"不支持的像素格式: {pdfFormat}")
            };
        }

        public Bitmap ToBitmap(PdfBitmap pdfBitmap)
        {
            if (pdfBitmap == null)
                throw new ArgumentNullException(nameof(pdfBitmap));

            if (pdfBitmap.IsDisposed)
                throw new ObjectDisposedException(nameof(pdfBitmap));

            // 获取对应的GDI+像素格式
            var pixelFormat = GetPixelFormat(pdfBitmap.Format);

            // 创建目标Bitmap
            var bitmap = new Bitmap(pdfBitmap.Width, pdfBitmap.Height, pixelFormat);

            // 锁定Bitmap的像素缓冲区
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.WriteOnly,
                pixelFormat);

            try
            {
                // 计算需要复制的字节总数
                int byteCount = pdfBitmap.Stride * pdfBitmap.Height;

                // 修正：使用接受源指针的Marshal.Copy重载
                // 从非托管内存指针复制到目标指针
                unsafe
                {
                    Buffer.MemoryCopy(
                        (void*)pdfBitmap.Buffer,    // 源非托管内存指针
                        (void*)bitmapData.Scan0,    // 目标非托管内存指针
                        byteCount,                  // 目标内存大小
                        byteCount);                 // 要复制的字节数
                }

                // 兼容旧版.NET的替代方案（不使用unsafe）：
                // IntPtr sourcePtr = pdfBitmap.Buffer;
                // IntPtr targetPtr = bitmapData.Scan0;
                // for (int i = 0; i < byteCount; i++)
                // {
                //     Marshal.WriteByte(targetPtr, i, Marshal.ReadByte(sourcePtr, i));
                // }
            }
            finally
            {
                // 解锁像素缓冲区
                bitmap.UnlockBits(bitmapData);
            }

            // 处理索引色图像的调色板
            if (pixelFormat == PixelFormat.Format8bppIndexed ||
                pixelFormat == PixelFormat.Format1bppIndexed)
            {
                SetPalette(bitmap, pdfBitmap);
            }

            return bitmap;
        }

        private void SetPalette(Bitmap bitmap, PdfBitmap pdfBitmap)
        {
            var palette = bitmap.Palette;
            int paletteSize = pdfBitmap.PaletteSize;

            for (int i = 0; i < paletteSize && i < palette.Entries.Length; i++)
            {
                // 将PDF调色板颜色转换为GDI+颜色
                var pdfColor = pdfBitmap.GetPaletteColorByIndex(i);
                palette.Entries[i] = System.Drawing.Color.FromArgb(
                    pdfColor.A, pdfColor.R, pdfColor.G, pdfColor.B);
            }

            bitmap.Palette = palette;
        }

        private string ExtractTextFromPdf(MemoryStream pdfStream)
        {
            if (pdfStream == null || pdfStream.Length == 0)
                throw new ArgumentException("PDF流不能为空", nameof(pdfStream));

            var textBuilder = new System.Text.StringBuilder();

            pdfStream.Position = 0; // 确保流位置在开始处

            // 使用 PdfPig 打开 PDF
            using (var document = UglyToad.PdfPig.PdfDocument.Open(pdfStream))
            {
                // 遍历每一页提取文本
                foreach (var page in document.GetPages())
                {
                    var pageText = page.Text;
                    if (!string.IsNullOrEmpty(pageText))
                    {
                        textBuilder.Append(pageText);
                    }
                }
            }
            return textBuilder.ToString();
        }
        /// <summary>
        /// 主函数：根据 PDF 类型调用相应方法
        /// </summary>
        public string ProcessPdf(MemoryStream pdfStream)
        {
            pdfStream.Position = 0;
            string content = ExtractTextFromPdf(pdfStream);
            if (!string.IsNullOrEmpty(content))
            {
                return content;
            }
            else
            {
                Console.WriteLine("Detected: Scanned/Image PDF. Extracting text using OCR...");
                string text = ExtractTextFromScannedPdf(pdfStream);
                Console.WriteLine($"Text:{text}");
                string cleanedText = CleanText(text);
                Console.WriteLine($"cleanedText:{cleanedText}");
                return cleanedText;
            }
        }

        private string CleanText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // 1. 去除所有空格（包括普通空格、不间断空格等）
            text = Regex.Replace(text, @"\s+", "");

            // 2. 去除控制字符（如换行、制表符等）
            text = Regex.Replace(text, @"[\x00-\x1F\x7F]", "");

            // 3. 去除重复的标点符号（可选，根据需求调整）
            text = Regex.Replace(text, @"([.,;()（），。；：:！!？?])\1+", "$1");

            // 4. 去除首尾无效字符
            text = text.Trim();

            return text;
        }
    }
}
