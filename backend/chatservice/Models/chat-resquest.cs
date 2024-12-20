using System.Text.Json.Serialization;

namespace chat_service.Models
{
    public class ChatRequest
    {
        public string Model { get; set; }
        public List<Message> Messages { get; set; }
        public bool Stream { get; set; }
    }


    public class LLMResponse
    {
        [JsonPropertyName("model")]
        public string model { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime created_at { get; set; }

        [JsonPropertyName("message")]
        public LLMMessage message { get; set; }

        [JsonPropertyName("done_reason")]
        public string done_reason { get; set; }

        [JsonPropertyName("done")]
        public bool done { get; set; }

        [JsonPropertyName("total_duration")]
        public long total_duration { get; set; }

        [JsonPropertyName("load_duration")]
        public long load_duration { get; set; }

        [JsonPropertyName("prompt_eval_count")]
        public int prompt_eval_count { get; set; }

        [JsonPropertyName("prompt_eval_duration")]
        public long prompt_eval_duration { get; set; }

        [JsonPropertyName("eval_count")]
        public int eval_count { get; set; }

        [JsonPropertyName("eval_duration")]
        public long eval_duration { get; set; }
    }

    public class LLMMessage
    {
        [JsonPropertyName("role")]
        public string role { get; set; }

        [JsonPropertyName("content")]
        public string content { get; set; }
    }


}

