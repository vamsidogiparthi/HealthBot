namespace HealthCareAgent.Brain.Plugins.FunctionPlugins;

public class UserIntentPlugin(
    IOptions<OpenAIConfiguration> openAIConfiguration,
    ILogger<UserIntentPlugin> logger
)
{
    [KernelFunction("get_user_intent")]
    [Description("Recognizes and provides user intent")]
    public async Task<string> GetUserIntent(
        [Description("user current chat message")] string userMessage,
        [Description("chathistory")] string summarizedHistory,
        Kernel kernel
    )
    {
        logger.LogInformation("Recognizing user input");
        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Required(),
            ModelId = openAIConfiguration.Value.Model,
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
        };

        var handlebarsPromptYaml = EmbeddedResource.Read("PromptTemplateTest.yaml");
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
