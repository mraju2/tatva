using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using chat_service.Models;

public class ChatService
{
    private readonly HttpClient _httpClient;
    private readonly DatabaseRepository _databaseRepository;

    private const int MAX_RETRIES = 3;


    public ChatService(HttpClient httpClient, DatabaseRepository databaseRepository)
    {
        _httpClient = httpClient;
        _databaseRepository = databaseRepository;
    }

    public async Task<LLMResponse> SendChatRequestAsync(UiRequest request)
    {
        Console.WriteLine("Step 1: Reading system message content from file...");

        // Step 1: Read system message content from file
        var systemMessageContent = await File.ReadAllTextAsync("system_prompt_sql.txt");
        Console.WriteLine("System message content read successfully.");

        int retryCount = 0;
        Exception lastException = null;

        Console.WriteLine("System and User messages created.");

        var messages = new List<Message>
        {
            new Message { Role = "system", Content = systemMessageContent },
            new Message { Role = "user", Content = request.UserMessage }
        };
        string sqlQuery = "";
        Console.WriteLine("ChatRequest object created. Sending request to LLM endpoint...");
        while (retryCount < MAX_RETRIES)
        {
            try
            {
                var chatRequest = new ChatRequest
                {
                    Model = request.Model,
                    Messages = messages,
                    Stream = false
                };

                Console.WriteLine($"Attempt {retryCount + 1}: Sending request to LLM endpoint...");


                // Step 2: Send request to the LLM endpoint
                var initialResponse = await SendChatRequestToEndpointAsync(chatRequest);
                Console.WriteLine("Received response from LLM endpoint.");
                Console.WriteLine($"LLM Response Content: {initialResponse.message.content}");

                // Step 3: Extract SQL Query
                Console.WriteLine("Extracting SQL query from the LLM response...");
                sqlQuery = ExtractSqlQuery(initialResponse.message.content);
                Console.WriteLine($"Extracted SQL Query: {sqlQuery}");

                // Step 4: Execute the SQL Query
                Console.WriteLine("Executing the extracted SQL query against the database...");
                var queryResults = await _databaseRepository.ExecuteQueryAsync(sqlQuery);
                Console.WriteLine("Query executed successfully. Results retrieved from the database:");
                Console.WriteLine(JsonSerializer.Serialize(queryResults, new JsonSerializerOptions { WriteIndented = true }));

                // Step 5: Beautify Results
                Console.WriteLine("Beautifying query results...");
                LLMResponse beautifiedResults = await BeautifyResultsAsync(queryResults, request.Model);
                Console.WriteLine("Results beautified successfully:");
                Console.WriteLine($"Response: {beautifiedResults.message}");
                Console.WriteLine(beautifiedResults);

                // Return the beautified results
                Console.WriteLine("Returning beautified results to the caller...");
                return beautifiedResults;
            }
            catch (Exception ex)
            {
                lastException = ex;
                retryCount++;

                if (retryCount >= MAX_RETRIES)
                {
                    break;
                }

                // Add error feedback to messages for next attempt
                messages.Add(new Message
                {
                    Role = "user",
                    Content = $@"The previous SQL query resulted in an error. Please correct the query based on the following details:
Error: {ex.Message}
Previous query: {sqlQuery}

Provide a corrected query with **Start** and **End** markers."
                });

                Console.WriteLine($"Attempt {retryCount} failed. Error: {ex.Message}");
                Console.WriteLine("Retrying with error feedback...");

                // Add a delay before retry
                await Task.Delay(1000 * retryCount); // Exponential backoff
            }
        }
        throw new Exception($"Failed to execute SQL query after {MAX_RETRIES} attempts. Last error: {lastException?.Message}", lastException);
    }

    private async Task<LLMResponse> SendChatRequestToEndpointAsync(ChatRequest chatRequest)
    {
        var requestBody = JsonSerializer.Serialize(chatRequest);
        var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

        var initialResponse = await _httpClient.PostAsync("http://localhost:11434/api/chat", requestContent);

        if (!initialResponse.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error: {initialResponse.StatusCode}, Message: {await initialResponse.Content.ReadAsStringAsync()}");
        }

        var initialResponseContent = await initialResponse.Content.ReadAsStringAsync();
        var llmResponse = JsonSerializer.Deserialize<LLMResponse>(initialResponseContent);

        if (llmResponse?.message?.content == null)
        {
            throw new Exception("No valid content returned by LLM.");
        }

        return llmResponse;
    }

    private string ExtractSqlQuery(string messageContent)
    {
        var startIndex = messageContent.IndexOf("**Start**", StringComparison.Ordinal);
        var endIndex = messageContent.IndexOf("**End**", StringComparison.Ordinal);

        if (startIndex >= 0 && endIndex > startIndex)
        {
            startIndex += "**Start**".Length; // Adjust to exclude marker
            return messageContent.Substring(startIndex, endIndex - startIndex).Trim();
        }

        throw new Exception("SQL query markers **Start** and **End** were not found.");
    }

    // Keep the other methods from the previous implementation:
    // - CreateResultBeautificationPrompt
    // - BeautifyResultsAsync
    // - FormatResultsAsDefaultMarkdownTable    
    private Message CreateResultBeautificationPrompt(List<Dictionary<string, object>> results)
    {
        var serializedResults = JsonSerializer.Serialize(results);

        return new Message
        {
            Role = "user",
            Content = $@"Convert this JSON data to a markdown table.

```json
{serializedResults}
```

CRITICAL REQUIREMENTS:
1. Output ONLY the markdown table
2. Include ALL rows
3. Do not add ANY explanatory text
4. Do not add '...' or placeholders
5. Do not add any comments before or after the table
6. Do not mention data formatting or conversion process
7. Do not add phrases like 'and so on' or 'rest of the data'
8. The response should start with '|' and end with '|'
9. ONLY create the table, nothing else"
        };
    }

    private async Task<LLMResponse> BeautifyResultsAsync(List<Dictionary<string, object>> results, string model)
    {
        if (results == null || results.Count == 0)
            return new LLMResponse
            {
                message = new LLMMessage
                {
                    content = "No results found.",
                    role = "assistant"
                }
            };

        var beautificationMessage = CreateResultBeautificationPrompt(results);
        var systemMessage = new Message
        {
            Role = "system",
            Content = @"You are a markdown table generator that:
1. Creates ONLY the markdown table
2. Starts response with '|' character
3. Ends response with '|' character
4. Never adds explanatory text
5. Never adds comments
6. Never mentions data processing
7. Never uses placeholders or '...'
8. Includes all data rows
9. Creates exact representation of the data"
        };

        var beautificationRequest = new ChatRequest
        {
            Model = model,
            Messages = new List<Message>
            {
                systemMessage,
                beautificationMessage
            },
            Stream = false
        };

        try
        {
            var requestBody = JsonSerializer.Serialize(beautificationRequest);
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:11434/api/chat", requestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var llmResponse = JsonSerializer.Deserialize<LLMResponse>(responseContent);

            return llmResponse;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to format results as markdown table", ex);
        }
    }

    private string CreateBasicMarkdownTable(List<Dictionary<string, object>> results)
    {
        if (!results.Any()) return "No results found.";

        var headers = results[0].Keys.ToList();
        var sb = new StringBuilder();

        // Create header row
        sb.AppendLine("| " + string.Join(" | ", headers) + " |");

        // Create separator row
        sb.AppendLine("| " + string.Join(" | ", headers.Select(_ => "---")) + " |");

        // Create data rows
        foreach (var row in results)
        {
            var values = headers.Select(h => row[h]?.ToString() ?? "NULL");
            sb.AppendLine("| " + string.Join(" | ", values) + " |");
        }

        return sb.ToString();
    }

    private string FormatResultsAsTable(List<Dictionary<string, object>> results)
    {
        if (results == null || results.Count == 0)
        {
            return "No results found.";
        }

        // Extract column headers
        var headers = results.First().Keys.ToList();

        // Build the table
        var table = new StringBuilder();

        // Add headers
        table.AppendLine(string.Join(" | ", headers));
        table.AppendLine(new string('-', headers.Count * 20));

        // Add rows
        foreach (var row in results)
        {
            var rowValues = headers.Select(header => row[header]?.ToString() ?? "NULL");
            table.AppendLine(string.Join(" | ", rowValues));
        }

        return table.ToString();
    }

    private Message CreateMessage(string role, string content)
    {
        return new Message
        {
            Role = role,
            Content = content
        };
    }

}


