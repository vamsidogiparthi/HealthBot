using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthCareAgent.Brain.Services;

public interface IMedicalProviderAPIService
{
    Task<string> SearchProvidersAsync(string zipcode, string? specializations = "");
}

public record APICondition
{
    [JsonPropertyName("resource")]
    public string Resource { get; init; } = "t";

    [JsonPropertyName("property")]
    public string Property { get; init; } = "record_id";

    [JsonPropertyName("value")]
    public string Value { get; init; } = string.Empty;

    [JsonPropertyName("operator")]
    public string Operator { get; init; } = "eq";
}

public record ConditionGroup
{
    [JsonPropertyName("conditions")]
    public List<APICondition> Conditions { get; init; } = [];

    [JsonPropertyName("limit")]
    public int Limit { get; init; } = 100;
}

public class MedicalProviderAPIService(
    IHttpClientFactory httpClientFactory,
    IOptions<MedicalProviderAPIConfiguration> options,
    ILogger<MedicalProviderAPIService> logger
) : IMedicalProviderAPIService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly MedicalProviderAPIConfiguration _options = options.Value;

    public async Task<string> SearchProvidersAsync(string zipcode, string? specializations = "")
    {
        var client = _httpClientFactory.CreateClient();
        var requestUrl = $"{_options.BaseUrl}/{_options.DistributionId}";
        var data = new ConditionGroup()
        {
            Conditions =
            [
                new APICondition
                {
                    Resource = "t",
                    Property = "zip_code",
                    Value = zipcode,
                    Operator = "starts with",
                },
            ],
            Limit = 5,
        };
        string json = JsonSerializer.Serialize(data);
        StringContent content = new(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(requestUrl, content);
        logger.LogInformation("Response status code: {StatusCode}", response.StatusCode);
        logger.LogInformation(
            "Response content: {Content}",
            await response.Content.ReadAsStringAsync()
        );
        return await response.Content.ReadAsStringAsync();
    }
}
