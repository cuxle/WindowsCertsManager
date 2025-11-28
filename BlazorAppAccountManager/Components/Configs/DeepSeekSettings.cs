using Newtonsoft.Json;

namespace BlazorAppAccountManager.Components.Configs
{
    public enum ModelType
    {
        DeepSeek,
        CherryGpt
    }

    public class ModelGlobalSettings
    {
        public string DefaultModelType { get; set; } = "DeepSeek";

        // DeepSeek 专属配置
        public DeepSeekSettings DeepSeek { get; set; } = new DeepSeekSettings();

        // CherryGPT 专属配置
        public CherryGptSettings CherryGPT { get; set; } = new CherryGptSettings();
    }

    public class DeepSeekSettings
    {
        // API相关配置
        public string ApiKey { get; set; }
        public string Endpoint { get; set; }
        public string Model { get; set; } = "deepseek-chat";

        // 文本处理配置
        public int MaxTextLength { get; set; } = 15000;

        // 系统提示词配置
        public string SystemPrompt { get; set; }

        public string UserMessage { get; set; }
    }

    // CherryGPT 配置类（复用之前的定义，补充必填校验）
    public class CherryGptSettings
    {
        public string ApiKey { get; set; } = string.Empty;

        public string Endpoint { get; set; } = "https://uat.cherygpt.com/v1/chat-messages";

        public string SystemPrompt { get; set; } = "你是一个专业的PDF关键词提取助手";

        public string UserMessage { get; set; } = "请提取指定关键词的相关信息";

        public string Model { get; set; } = "cherygpt-pro";

        public int MaxTextLength { get; set; } = 15000;
    }


    // cherry gpt request / response class
    // 先定义适配新接口的请求/响应模型
    public class CherryGptNewRequest
    {
        [JsonProperty("inputs")]
        public object Inputs { get; set; } = new { }; // 按文档要求传入空对象或自定义键值对

        [JsonProperty("query")]
        public string Query { get; set; } = string.Empty;

        [JsonProperty("response_mode")]
        public string ResponseMode { get; set; } = "streaming"; // 文档推荐streaming

        [JsonProperty("user")]
        public string User { get; set; } = string.Empty;

        [JsonProperty("files")]
        public List<CherryGptFile>? Files { get; set; }
    }

    public class CherryGptFile
    {
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("transfer_method")]
        public string TransferMethod { get; set; } = string.Empty;

        [JsonProperty("upload_file_id")]
        public string UploadFileId { get; set; } = string.Empty;
    }

    // 响应模型（需根据接口实际返回调整，这里匹配文档的"code/message/data"风格）
    public class CherryGptNewResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("data")]
        public CherryGptNewResponseData? Data { get; set; }
    }

    public class CherryGptNewResponseData
    {
        [JsonProperty("content")]
        public string? Content { get; set; }
        // 其他字段根据接口返回补充
    }

    // 基础事件模型（仅用于判断event类型）
    public class BaseEvent
    {
        [JsonProperty("event")]
        public string Event { get; set; }
    }

    public class WorkflowFinishedEvent
    {
        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("conversation_id")]
        public string ConversationId { get; set; }

        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("task_id")]
        public string TaskId { get; set; }

        [JsonProperty("workflow_run_id")]
        public string WorkflowRunId { get; set; }

        [JsonProperty("data")]
        public WorkflowData Data { get; set; }
    }

    // 嵌套的data字段模型
    public class WorkflowData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("workflow_id")]
        public string WorkflowId { get; set; }

        [JsonProperty("sequence_number")]
        public int SequenceNumber { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; } // 如"succeeded"

        [JsonProperty("outputs")]
        public WorkflowOutputs Outputs { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; } // 失败时为错误信息，成功时为null

        [JsonProperty("elapsed_time")]
        public double ElapsedTime { get; set; } // 耗时（秒）

        [JsonProperty("total_tokens")]
        public int TotalTokens { get; set; }

        [JsonProperty("total_steps")]
        public int TotalSteps { get; set; }

        [JsonProperty("created_by")]
        public CreatedBy CreatedBy { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("finished_at")]
        public long FinishedAt { get; set; }

        [JsonProperty("files")]
        public List<object> Files { get; set; }
    }

    // 嵌套的outputs字段模型（核心是answer）
    public class WorkflowOutputs
    {
        [JsonProperty("answer")]
        public string Answer { get; set; } // 最终回复内容
    }

    // 嵌套的created_by字段模型
    public class CreatedBy
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }
    }
}