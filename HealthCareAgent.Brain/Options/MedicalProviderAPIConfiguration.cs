namespace HealthCareAgent.Brain.Options;

public class MedicalProviderAPIConfiguration
{
    public const string SectionName = "MedicalProviderAPI";
    public string BaseUrl { get; set; } = string.Empty;
    public string DistributionId { get; set; } = string.Empty;
}
