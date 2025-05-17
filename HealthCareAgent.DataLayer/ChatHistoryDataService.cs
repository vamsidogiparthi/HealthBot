using Microsoft.SemanticKernel.ChatCompletion;

namespace HealthCareAgent.DataLayer;

public interface IChatHistoryDataService
{
    public Task<UserChatHistory> GetChatHistoryByUser(string userConnectionId);

    public Task SaveChatHistory(string userConnectionId, ChatHistory chatHistorySummary);
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
        return await _userChatHistory.Find(filter).FirstOrDefaultAsync();
    }

    public async Task SaveChatHistory(string userConnectionId, ChatHistory chatHistory)
    {
        _logger.LogInformation("Saving User Chat History");
        ArgumentNullException.ThrowIfNull(userConnectionId);
        ArgumentNullException.ThrowIfNull(chatHistory);

        var userChatHistory = await GetChatHistoryByUser(userConnectionId) ?? new UserChatHistory();
        userChatHistory.ChatHistory = chatHistory;

        await _userChatHistory.InsertOneAsync(userChatHistory);
    }
}
