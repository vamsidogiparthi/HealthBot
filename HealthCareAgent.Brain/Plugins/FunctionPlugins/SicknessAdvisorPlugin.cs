namespace HealthCareAgent.Brain.Plugins.FunctionPlugins;

public class SicknessAdvisorPlugin(
    ILogger<SicknessAdvisorPlugin> logger,
    IOptions<OpenAIConfiguration> options
// IOptions<GoogleCustomSearchConfiguration> googleSearchOptions,
)
{
    [KernelFunction("provide_sickness_advice")]
    [Description(
        "This function is responsible for providing advice on sickness based on user input and medical knowledge"
    )]
    public async Task<string> GetSicknessAdvise(
        [Description("user current chat message")] string userMessage,
        [Description("user agent summarized chat history")] string summarizedHistory,
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

        var handlebarsPromptYaml = EmbeddedResource.Read("SicknessAdvicePromptTemplate.yaml");
        var templateFactory = new HandlebarsPromptTemplateFactory();
        var function = kernel.CreateFunctionFromPromptYaml(handlebarsPromptYaml, templateFactory);

        var arguments = new KernelArguments()
        {
            { "userInput", userMessage },
            { "chatHistory", summarizedHistory },
            { "executionSettings", openAIPromptExecutionSettings },
        };

        var response = await kernel.InvokeAsync(function, arguments);

        logger.LogInformation("User input recognized {response}", response.ToString());
        return response.ToString();
    }
}
