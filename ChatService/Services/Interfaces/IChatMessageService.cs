using ChatService.Models;
using System.Threading.Tasks;

namespace ChatService.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for chat-related operations.
    /// </summary>
    public interface IChatMessageService
    {
        /// <summary>
        /// Sends a chat request to the chat system.
        /// </summary>
        /// <param name="request">The chat request.</param>
        /// <returns>The chat response.</returns>
        Task<LLMResponse> SendChatRequestAsync(UserChatRequest request);
    }
}
