using ChatService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatService.Repositories
{
    public interface IMongoRepository
    {
        /// <summary>
        /// Saves chat history to the MongoDB collection.
        /// </summary>
        Task SaveChatHistoryAsync(ChatHistoryModel chatHistory);

        /// <summary>
        /// Retrieves a list of chat history with an optional limit on the number of results.
        /// </summary>
        Task<List<ChatHistoryModel>> GetChatHistoryAsync(int limit = 100);

        /// <summary>
        /// Retrieves user-specific chat history based on a user message search term.
        /// </summary>
        Task<List<ChatHistoryModel>> GetUserChatHistoryAsync(string userMessage, int limit = 10);
    }
}
