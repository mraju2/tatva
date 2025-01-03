using System.Text.Json.Serialization;

namespace ChatService.Models
{
    /// <summary>
    /// Represents a chat request sent to the LLM.
    /// </summary>
    public class OllamaChatRequest
    {
        /// <summary>
        /// The model to be used for generating responses.
        /// </summary>
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// A list of messages in the conversation.
        /// </summary>
        public List<LLMMessage> Messages { get; set; } = new();

        /// <summary>
        /// Indicates whether the response should be streamed.
        /// </summary>
        public bool Stream { get; set; }
    }
    
}