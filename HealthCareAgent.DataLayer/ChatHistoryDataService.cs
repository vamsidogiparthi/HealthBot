namespace HealthCareAgent.DataLayer;

public interface IChatHistoryDataService
{
    public Task<string> GetChatHistoryByUser(string userConnectionId);

    public Task<int> SaveChatHistory(string userConnectionId, string chatHistorySummary);
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

    public Task<string> GetChatHistoryByUser(string userConnectionId)
    {
        _logger.LogInformation("Fetching user chat history");
        throw new NotImplementedException();
    }

    public Task<int> SaveChatHistory(string userConnectionId, string chatHistorySummary)
    {
        _logger.LogInformation("Saving User Chat History");
        throw new NotImplementedException();
    }
}
