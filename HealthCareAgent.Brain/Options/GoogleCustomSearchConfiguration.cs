namespace HealthCareAgent.Brain.Options;

public class GoogleCustomSearchConfiguration
{
    public const string SectionName = "GoogleCustomSearchConfiguration";

    public string ApiKey { get; set; } = string.Empty;
    public string SearchEngineId { get; set; } = string.Empty;
    public int MaxResults { get; set; } = 10;
    public string SearchUrl { get; set; } = "https://www.googleapis.com/customsearch/v1";
}
