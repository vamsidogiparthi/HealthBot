using Microsoft.SemanticKernel.Data;
using Microsoft.SemanticKernel.Plugins.Web.Bing;
using Microsoft.SemanticKernel.Plugins.Web.Google;

namespace HealthCareAgent.Brain.Plugins.FunctionPlugins;

public class MedicalProviderDatabasePlugin(
    ILogger<MedicalProviderDatabasePlugin> logger,
    IOptions<OpenAIConfiguration> options,
    // IOptions<GoogleCustomSearchConfiguration> googleSearchOptions,
    IMedicalProviderAPIService medicalProviderAPIService
)
{
    // [KernelFunction("get_medical_providers")]
    // [Description(
    //     "This function is responsible for initiating the provider information data when needed during the chat"
    // )]
    // public async Task<string> GetListofMedicalProviders(
    //     [Description("user chat message")] string userMessage,
    //     [Description("doctor specializations needed to be pulled")] string specializes,
    //     [Description("user location zipcode for searches")] string userZipcode,
    //     Kernel kernel
    // )
    // {
    //     logger.LogInformation("Recognizing user input");
    //     OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
    //     {
    //         FunctionChoiceBehavior = FunctionChoiceBehavior.Required(),
    //         ModelId = options.Value.Model,
    //         ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
    //     };

    //     var handlebarsPromptYaml = EmbeddedResource.Read("PromptTemplateTest.yaml");
    //     var templateFactory = new HandlebarsPromptTemplateFactory();
    //     var function = kernel.CreateFunctionFromPromptYaml(handlebarsPromptYaml, templateFactory);

    //     var arguments = new KernelArguments()
    //     {
    //         { "userInput", userMessage },
    //         { "specializes", specializes },
    //         { "userZipcode", userZipcode },
    //         { "executionSettings", openAIPromptExecutionSettings },
    //     };

    //     var response = await kernel.InvokeAsync(function, arguments);

    //     logger.LogInformation("User input recognized {response}", response.ToString());
    //     return response.ToString();
    // }

    [KernelFunction("search_providers")]
    [Description("This function conducts a web search using bing for near by medical providers")]
    public async Task<string> ConductProviderSearch(
        [Description("user provided zipcode")] string zipcode,
        [Description("determined specialization of the medical provider")] string specializes
    )
    {
        // #pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        //         var textSearch = new GoogleTextSearch(
        //             searchEngineId: googleSearchOptions.Value.SearchEngineId,
        //             apiKey: googleSearchOptions.Value.ApiKey
        //         );
        // #pragma warning restore SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        //         var searchPlugin = textSearch.CreateWithGetTextSearchResults("SearchPlugin");
        //         kernel.Plugins.Add(searchPlugin);
        //         var query = $"Near by medical providers in {zipcode}";
        //         string promptTemplate = """
        //             {{#with (SearchPlugin-GetTextSearchResults query)}}
        //                 {{#each this}}
        //                 Name: {{Name}}
        //                 Value: {{Value}}
        //                 Link: {{Link}}
        //                 -----------------
        //                 {{/each}}
        //             {{/with}}

        //             {{query}}

        //             Use Google Maps or real-time google search data. Include the name, address, phone number, and website of the medical providers in your response.
        //             If you cannot find any medical providers, respond with "No medical providers found in the specified area."
        //             """;
        //         KernelArguments arguments = new() { { "query", query } };
        //         HandlebarsPromptTemplateFactory promptTemplateFactory = new();
        //         // Build a text search plugin with Bing search and add to the kernel

        //         var response = await kernel.InvokePromptAsync(
        //             promptTemplate,
        //             arguments,
        //             templateFormat: HandlebarsPromptTemplateFactory.HandlebarsTemplateFormat,
        //             promptTemplateFactory: promptTemplateFactory
        //         );
        //         logger.LogInformation("Conducted provider search with response: {response}", response);
        //         return response.ToString();

        var response = await medicalProviderAPIService.SearchProvidersAsync(zipcode, specializes);
        logger.LogInformation("Conducted provider search with response: {response}", response);
        return response.ToString();
    }

    [KernelFunction("distinct_provider_specializes")]
    [Description(
        "This function retrieves distinct medical specializations from the provider database"
    )]
    public async Task<string[]> GetDistinctProviderSpecializes()
    {
        var response = await medicalProviderAPIService.MedicalSpecializationsAsync();
        logger.LogInformation("Conducted provider search with response: {response}", response);
        return response;
    }
}
