using Microsoft.SemanticKernel.Data;
using Microsoft.SemanticKernel.Plugins.Web.Bing;
using Microsoft.SemanticKernel.Plugins.Web.Google;

namespace HealthCareAgent.Brain.Plugins.FunctionPlugins;

public class MedicalProviderDatabasePlugin(
    ILogger<MedicalProviderDatabasePlugin> logger,
    IOptions<OpenAIConfiguration> options,
    IOptions<GoogleCustomSearchConfiguration> googleSearchOptions
)
{
    [KernelFunction("get-medical-providers")]
    [Description(
        "This function is responsible for initiating the provider information data when needed during the chat"
    )]
    public async Task<string> GetListofMedicalProviders(
        [Description("user chat message")] string userMessage,
        [Description("doctor specializations needed to be pulled")] string specializes,
        [Description("user location zipcode for searches")] string userZipcode,
        Kernel kernel
    )
    {
        logger.LogInformation("Recognizing user input");
        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Required(),
            ModelId = options.Value.Model,
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
        };

        var handlebarsPromptYaml = EmbeddedResource.Read("PromptTemplateTest.yaml");
        var templateFactory = new HandlebarsPromptTemplateFactory();
        var function = kernel.CreateFunctionFromPromptYaml(handlebarsPromptYaml, templateFactory);

        var arguments = new KernelArguments()
        {
            { "userInput", userMessage },
            { "specializes", specializes },
            { "userZipcode", userZipcode },
            { "executionSettings", openAIPromptExecutionSettings },
        };

        var response = await kernel.InvokeAsync(function, arguments);

        logger.LogInformation("User input recognized {response}", response.ToString());
        return response.ToString();
    }

    [KernelFunction("search-providers")]
    [Description("This function conducts a web search using bing for near by medical providers")]
    public async Task<string> ConductProviderSearch(
        [Description("user provided zipcode")] string zipcode,
        Kernel kernel
    )
    {
#pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        var textSearch = new GoogleTextSearch(
            searchEngineId: googleSearchOptions.Value.SearchEngineId,
            apiKey: googleSearchOptions.Value.ApiKey
        );
#pragma warning restore SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        var query = $"Search for medical providers, doctors, hospital by zipcode = {zipcode}";
        var prompt = "{{GoogleSearchPlugin.Search $query}}. {{$query}}";
        // Build a text search plugin with Bing search and add to the kernel
        var searchPlugin = textSearch.CreateWithSearch("GoogleSearchPlugin");
        kernel.Plugins.Add(searchPlugin);

        var arguments = new KernelArguments() { { "query", query } };

        var response = await kernel.InvokePromptAsync(prompt, arguments);

        return response.ToString();
    }
}
