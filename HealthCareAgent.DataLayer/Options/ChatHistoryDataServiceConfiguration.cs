namespace HealthCareAgent.DataLayer.Options;

public class ChatHistoryDataServiceConfiguration
{
    public const string SectionName = "ChatHistoryDataServiceConfiguration";
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string CollectionName { get; set; } = string.Empty;
}
