using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ChatService.Models;
using ChatService.Repositories;
using ChatService.Services.Interfaces;
using ChatService.Utilities;
using MongoDB.Bson;
using Microsoft.Extensions.Logging;

namespace ChatService.Services
{
    /// <summary>
    /// Provides methods for sending chat requests and processing responses.
    /// </summary>
    public class ChatMessageService : IChatMessageService
    {
        private readonly HttpClient _httpClient;
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IMongoRepository _mongoRepository;
        private readonly MarkdownTableCreator _markdownTableCreator;
        private readonly ILogger<ChatMessageService> _logger;

        private const int MAX_RETRIES = 3;

        public ChatMessageService(
            HttpClient httpClient,
            IDatabaseRepository databaseRepository,
            IMongoRepository mongoRepository,
            MarkdownTableCreator markdownTableCreator,
            ILogger<ChatMessageService> logger)
        {
            _httpClient = httpClient;
            _databaseRepository = databaseRepository;
            _mongoRepository = mongoRepository;
            _markdownTableCreator = markdownTableCreator;
            _logger = logger;
        }

        public async Task<LLMResponse> SendChatRequestAsync(UserChatRequest request)
        {
            try
            {
                _logger.LogInformation("Initializing chat request for user message: {UserMessage}", request.UserMessage);

                var chatHistory = new ChatHistoryModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Timestamp = DateTime.UtcNow,
                    UserMessage = request.UserMessage,
                    Model = request.Model,
                    IsSuccessful = false
                };

                _logger.LogInformation("Reading system prompt from file...");
                var systemMessageContent = await File.ReadAllTextAsync("system_prompt_sql.txt");
                _logger.LogInformation("System prompt loaded successfully.");

                var messages = new List<LLMMessage>
                {
                    new LLMMessage { Role = "system", Content = systemMessageContent },
                    new LLMMessage { Role = "user", Content = request.UserMessage }
                };

                string sqlQuery = string.Empty;
                Exception lastException = null;

                for (int retryCount = 0; retryCount < MAX_RETRIES; retryCount++)
                {
                    try
                    {
                        var chatRequest = new OllamaChatRequest
                        {
                            Model = request.Model,
                            Messages = messages,
                            Stream = false
                        };

                        _logger.LogInformation("Sending request to LLM endpoint. Attempt {Attempt}", retryCount + 1);
                        var initialResponse = await SendChatRequestToEndpointAsync(chatRequest);
                        chatHistory.LLMResponse = initialResponse.Message.Content;

                        _logger.LogInformation("Received response from LLM endpoint: {Content}", initialResponse.Message.Content);

                        _logger.LogInformation("Extracting SQL query from LLM response...");
                        sqlQuery = ExtractSqlQuery(initialResponse.Message.Content);
                        chatHistory.SqlQuery = sqlQuery;
                        initialResponse.SqlQuery = sqlQuery;

                        _logger.LogInformation("Executing SQL query against the database...");
                        var queryResults = await _databaseRepository.ExecuteQueryAsync(sqlQuery);
                        _logger.LogInformation("Query executed successfully. Retrieved {RowCount} rows.", queryResults.Count);

                        var markdownResults = _markdownTableCreator.CreateMarkdownTable(queryResults);
                        chatHistory.QueryResults = markdownResults;
                        chatHistory.IsSuccessful = true;

                        await _mongoRepository.SaveChatHistoryAsync(chatHistory);

                        _logger.LogInformation("Returning formatted markdown results.");
                        initialResponse.Message.Content = markdownResults;

                        return initialResponse;
                    }
                    catch (Exception retryException)
                    {
                        _logger.LogError(retryException, "Error during chat processing. Attempt {Attempt} failed.", retryCount + 1);
                        lastException = retryException;

                        if (retryCount >= MAX_RETRIES - 1)
                            break;

                        messages.Add(new LLMMessage
                        {
                            Role = "user",
                            Content = $@"The previous SQL query resulted in an error. Please correct the query based on the following details:
Error: {retryException.Message}
Previous query: {sqlQuery}

Provide a corrected query with **Start** and **End** markers."
                        });

                        await Task.Delay(1000 * (retryCount + 1)); // Exponential backoff
                    }
                }

                throw new Exception("Failed to process chat request after multiple retries.", lastException);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Critical error during chat request processing.");
                throw;
            }
        }

        private async Task<LLMResponse> SendChatRequestToEndpointAsync(OllamaChatRequest chatRequest)
        {
            try
            {
                var requestBody = JsonSerializer.Serialize(chatRequest);
                var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

                _logger.LogInformation("Sending request to LLM endpoint...");
                _httpClient.Timeout = TimeSpan.FromMinutes(5); // Increase timeout to 5 minutes
                var response = await _httpClient.PostAsync("http://159.223.173.153:11434/api/chat", requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"LLM API call failed: {response.StatusCode} - {errorMessage}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LLMResponse>(responseContent) ?? throw new Exception("Invalid response from LLM.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending chat request to LLM endpoint.");
                throw;
            }
        }

        private string ExtractSqlQuery(string messageContent)
        {
            try
            {
                var startIndex = messageContent.IndexOf("**Start**", StringComparison.Ordinal);
                var endIndex = messageContent.IndexOf("**End**", StringComparison.Ordinal);

                if (startIndex >= 0 && endIndex > startIndex)
                {
                    startIndex += "**Start**".Length;
                    return messageContent.Substring(startIndex, endIndex - startIndex).Trim();
                }

                throw new Exception("SQL query markers **Start** and **End** not found in response.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting SQL query from response.");
                throw;
            }
        }
    }
}
