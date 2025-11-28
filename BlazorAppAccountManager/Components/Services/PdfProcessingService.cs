using BlazorAppAccountManager.Components.Configs;
using BlazorAppAccountManager.Components.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using static BlazorAppAccountManager.Components.Pages.CertificateManagement;

namespace BlazorAppAccountManager.Components.Services
{
    public interface IPdfProcessingService
    {
        Task<(Dictionary<string, object> ParsedResult, string ErrorMsg)> ProcessPdfAsync(NativeBrowserFile file, List<string> keywords, CertificateType currentCertType);
    }

    public class PdfProcessingService : IPdfProcessingService
    {
        private readonly IOptions<TesseractConfig> _tesseractOptions;
        private readonly IOptions<ModelGlobalSettings> _modelGlobalSettings;
        private readonly IHttpClientFactory _httpClientFactory; // 新增：用于 CherryGPT HTTP 调用
        private readonly ILogger<PdfProcessingService> _logger;

        public PdfProcessingService(IOptions<TesseractConfig> tesseractOptions, 
            IOptions<ModelGlobalSettings> modelGlobalSettings,
            IHttpClientFactory httpClientFactory,
            ILogger<PdfProcessingService> logger)
        {
            _tesseractOptions = tesseractOptions;
            _modelGlobalSettings = modelGlobalSettings;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // 完整 PDF 处理（解析 + AI 提取）
        public async Task<(Dictionary<string, object> ParsedResult, string ErrorMsg)> 
            ProcessPdfAsync(NativeBrowserFile file, List<string> keywords, CertificateType currentCertType)
        {
            try
            {
                // 1. 读取文件流
                using var fileStream = await file.OpenReadStreamAsync(20 * 1024 * 1024); // 20MB 限制
                using var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                // 2. 提取文本（Tesseract + PDFPig）
                var tesseractPath = _tesseractOptions.Value.Path;
                var pdfProcessor = new PdfProcessor2(tesseractPath);
                var pdfText = pdfProcessor.ProcessPdf(memoryStream);
                memoryStream.Position = 0;

                var targetModelType = _modelGlobalSettings.Value.DefaultModelType;
                var maxTextLength = targetModelType switch
                {
                    "DeepSeek" => _modelGlobalSettings.Value.DeepSeek.MaxTextLength,
                    "CherryGPT" => _modelGlobalSettings.Value.CherryGPT.MaxTextLength,
                    _ => 20000
                };

                if (pdfText.Length > maxTextLength)
                {
                    pdfText = pdfText.Substring(0, maxTextLength);
                    _logger.LogWarning($"PDF文本超长，已截断至 {maxTextLength} 字符");
                }

                // 4. 调用 AI 提取关键词
                string jsonResult = "";
                switch (targetModelType)
                {
                    case "DeepSeek":
                        jsonResult = await QueryDeepSeekAsync(pdfText, keywords);
                        break;
                    case "CherryGPT":
                        jsonResult = await QueryCheryGptAsync(pdfText, keywords);
                        break;
                    default:
                        throw new NotSupportedException($"不支持的模型类型：{targetModelType}");
                }
                
                var parsedResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResult);

                // 5. 验证证书类型（提前处理不匹配场景） 放到后边处理
                //if (parsedResult.TryGetValue("证书类型", out var typeValue)
                //    && !string.IsNullOrEmpty(typeValue?.ToString())
                //    && Enum.TryParse<CertificateType>(typeValue.ToString(), out var parsedCertType)
                //    && parsedCertType != currentCertType && parsedCertType != CertificateType.Other)
                //{
                //    return (null, $"证书类型不匹配：解析为 {parsedCertType}，当前需 {currentCertType}");
                //}

                return (parsedResult, null);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "AI 返回结果 JSON 解析失败");
                return (null, "证书信息解析失败，请重试");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PDF 处理异常");
                return (null, $"处理失败：{ex.Message}");
            }
        }

        // 新增：CherryGPT 调用方法（基于 HTTP 原生调用）
        private async Task<string> QueryCheryGptAsync(string pdfText, List<string> keywords)
        {
            var settings = _modelGlobalSettings.Value.CherryGPT;
            var keywordsStr = string.Join(", ", keywords);

            // 构造query内容（对应文档的"query"参数）
            var userQuery = $"PDF内容：{pdfText}\n请提取以下关键词信息：{keywordsStr}。{settings.UserMessage}";

            _logger.LogInformation(userQuery);

            // 1. 构造请求体（严格匹配文档的Body Parameter）
            var requestBody = new CherryGptNewRequest
            {
                Inputs = new { }, // 文档要求传入，可传空对象
                Query = userQuery, // 用户输入内容（对应原messages的user部分）
                ResponseMode = "streaming", // 文档要求必填，推荐streaming
                User = "证书管理系统", // 文档要求必填：用户标识（需替换为实际业务的用户ID）
                Files = null // 若不需要传文件则为null；如需传PDF文件，需先上传获取upload_file_id
            };

            // 2. 创建 HTTP 请求
            using var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(5); // 超时时间
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                settings.ApiKey // 直接传入你的 API_KEY，无需拼接字符串
            );
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // 3. 序列化请求体
            var jsonBody = JsonConvert.SerializeObject(requestBody, Formatting.None);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // 4. 发送请求并处理响应
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(settings.Endpoint, content);
                var responseJson = await response.Content.ReadAsStringAsync();

                response.EnsureSuccessStatusCode(); // 处理HTTP错误

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream, Encoding.UTF8);

                string line;

                WorkflowFinishedEvent targetEvent = null;

                // 逐行读取流数据
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    // 过滤空行或无效格式行
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // （如果是SSE格式，需先去除"data: "前缀，根据实际格式调整）
                    // 示例：若行格式为 "data: {json}"，则先提取JSON
                    string json = line;
                    const string ssePrefix = "data: ";
                    if (line.StartsWith(ssePrefix))
                    {
                        json = line.Substring(ssePrefix.Length).Trim();
                        // 处理流结束标识（如 "[DONE]"）
                        if (json.Equals("[DONE]", StringComparison.OrdinalIgnoreCase))
                            break;
                    }

                    // 尝试解析JSON，判断是否为目标事件
                    try
                    {
                        // 先解析基础结构，获取event字段
                        var baseEvent = JsonConvert.DeserializeObject<BaseEvent>(json);
                        if (baseEvent?.Event == "workflow_finished")
                        {
                            // 匹配目标事件，解析完整数据
                            targetEvent = JsonConvert.DeserializeObject<WorkflowFinishedEvent>(json);
                            _logger.LogInformation($"捕获到 workflow_finished 事件：{json}");

                            // 若获取后无需继续处理，可直接退出循环（按需选择）
                            break;
                        }
                        // 非目标事件，直接忽略（不打印日志，减少干扰）
                    }
                    catch (JsonException ex)
                    {
                        // 忽略JSON格式错误的行（非目标事件无需关心）
                        _logger.LogDebug(ex, $"忽略无效JSON格式：{json}");
                        continue;
                    }
                }
                string result = "";
                if (targetEvent != null)
                {
                    result = targetEvent.Data.Outputs.Answer;
                    _logger.LogError(result);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CherryGPT 接口调用失败");
                throw new InvalidOperationException($"CherryGPT 调用失败：{ex.Message}");
            }
        }

        // 辅助类：CherryGPT 响应模型
        private class CheryGptResponse
        {
            [JsonProperty("code")]
            public int Code { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; } = string.Empty;

            [JsonProperty("data")]
            public CheryGptResponseData Data { get; set; } = new();
        }

        private class CheryGptResponseData
        {
            [JsonProperty("content")]
            public string Content { get; set; } = string.Empty;
        }

        // AI 调用（DeepSeek）
        private async Task<string> QueryDeepSeekAsync(string pdfText, List<string> keywords)
        {
            var settings = _modelGlobalSettings.Value.DeepSeek;
            var keywordsStr = string.Join(", ", keywords);
            var messages = new List<ChatMessage>
            {
                ChatMessage.CreateSystemMessage(settings.SystemPrompt),
                ChatMessage.CreateUserMessage($"PDF内容：{pdfText}"),
                ChatMessage.CreateUserMessage($"请提取以下关键词信息：{keywordsStr}。{settings.UserMessage}")
            };

            var client = new ChatClient(
                model: settings.Model,
                credential: new ApiKeyCredential(settings.ApiKey),
                options: new OpenAIClientOptions { Endpoint = new Uri(settings.Endpoint) }
            );

            var response = await client.CompleteChatAsync(messages);
            return response.Value.Content[0].Text;
        }
    }
}
