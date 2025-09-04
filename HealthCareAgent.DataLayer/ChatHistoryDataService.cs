using Microsoft.SemanticKernel.ChatCompletion;

namespace HealthCareAgent.DataLayer;

public interface IChatHistoryDataService
{
    public Task<UserChatHistory> GetChatHistoryByUser(string userConnectionId);

    public Task SaveChatHistory(string userConnectionId, History chatHistorySummary);
}

public class ChatHistoryDataService : IChatHistoryDataService
{
    private readonly IMongoCollection<UserChatHistory> _userChatHistory;
    private readonly ILogger<ChatHistoryDataService> _logger;

    public ChatHistoryDataService(
        IOptions<ChatHistoryDataServiceConfiguration> options,
        ILogger<ChatHistoryDataService> logger
    )
    {
        ArgumentNullException.ThrowIfNull(options);

        _logger = logger;

        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var database = mongoClient.GetDatabase(options.Value.DatabaseName);
        _userChatHistory = database.GetCollection<UserChatHistory>(options.Value.CollectionName);
    }

    public async Task<UserChatHistory> GetChatHistoryByUser(string userConnectionId)
    {
        _logger.LogInformation("Fetching user chat history");
        var filter = Builders<UserChatHistory>.Filter.Eq(
            up => up.UserChatConnectId,
            userConnectionId
        );
        var result = await _userChatHistory.Find(filter).FirstOrDefaultAsync();
        return result ?? new(userConnectionId);
    }

    public async Task SaveChatHistory(string userConnectionId, History chatHistory)
    {
        _logger.LogInformation("Saving User Chat History");
        ArgumentNullException.ThrowIfNull(userConnectionId);
        ArgumentNullException.ThrowIfNull(chatHistory);

        var filter = Builders<UserChatHistory>.Filter.Eq(
            up => up.UserChatConnectId,
            userConnectionId
        );

        var userChatHistory = await _userChatHistory.Find(filter).FirstOrDefaultAsync();

        if (userChatHistory is null)
            await _userChatHistory.InsertOneAsync(new(userConnectionId) { History = chatHistory });
        else
        {
            userChatHistory.History = chatHistory;
            await _userChatHistory.ReplaceOneAsync(filter, userChatHistory);
        }
    }
}
