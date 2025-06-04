using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HealthCareAgent.Brain.Models;

namespace HealthCareAgent.Brain.Services;

public interface IMedicalProviderAPIService
{
    Task<string> SearchProvidersAsync(string zipcode, string? specializations = "");
    Task<string[]> MedicalSpecializationsAsync()
    {
        var sample_data = EmbeddedResource.Read("Sample_Provider_Data.json");
        if (!string.IsNullOrEmpty(sample_data))
        {
            var deserializedData = JsonSerializer.Deserialize<MedAPIResponse>(sample_data);

            var distinctSpecializations = deserializedData
                ?.Results.Select(provider => provider.PriSpec)
                .Distinct()
                .ToArray();

            return Task.FromResult(distinctSpecializations ?? []);
        }
        return Task.FromResult(Array.Empty<string>());
    }
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

        if (specializations is not null && specializations.Length > 0)
        {
            data.Conditions.Add(
                new APICondition
                {
                    Resource = "t",
                    Property = "pri_spec",
                    Value = specializations,
                    Operator = "contains",
                }
            );
        }

        string json = JsonSerializer.Serialize(data);
        StringContent content = new(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(requestUrl, content);
        logger.LogInformation("Response status code: {StatusCode}", response.StatusCode);
        var responseContent = await response.Content.ReadFromJsonAsync<MedAPIResponse>();
        logger.LogInformation("Response content: {Content}", responseContent?.ToString() ?? "null");
        return await response.Content.ReadAsStringAsync();
    }
}
