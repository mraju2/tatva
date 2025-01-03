using ChatService.Models;
using ChatService.Configurations;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatService.Repositories
{
    public class MongoRepository : IMongoRepository
    {
        private readonly IMongoCollection<ChatHistoryModel> _collection;

        public MongoRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<ChatHistoryModel>(settings.Value.CollectionName);
        }

        public async Task SaveChatHistoryAsync(ChatHistoryModel chatHistory)
        {
            await _collection.InsertOneAsync(chatHistory);
        }

        public async Task<List<ChatHistoryModel>> GetChatHistoryAsync(int limit = 100)
        {
            return await _collection.Find(_ => true)
                .SortByDescending(x => x.Timestamp)
                .Limit(limit)
                .ToListAsync();
        }

        public async Task<List<ChatHistoryModel>> GetUserChatHistoryAsync(string userMessage, int limit = 10)
        {
            return await _collection.Find(x => x.UserMessage.Contains(userMessage))
                .SortByDescending(x => x.Timestamp)
                .Limit(limit)
                .ToListAsync();
        }
    }
}
