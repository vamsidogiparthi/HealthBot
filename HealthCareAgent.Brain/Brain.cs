using HealthCareAgent.DataLayer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.ChatCompletion;

namespace HealthCareAgent.Brain;

public interface IBrain
{
    Task<string> RunAsync(string userMessage, string userConnectionId);
    Task<string> RunMedicalProviderSearchAsync(string zipcode);
}

public class Brain(
    [FromKeyedServices("ChatBotKernel")] Kernel kernel,
    ILogger<Brain> logger,
    IChatHistoryDataService chatHistoryDataService
) : IBrain
{
    private readonly Kernel _kernel = kernel;
    private readonly ILogger<Brain> logger = logger;
    private readonly IChatHistoryDataService chatHistoryDataService = chatHistoryDataService;

    public async Task<string> RunAsync(string userMessage, string userConnectionId)
    {
        logger.LogInformation("Initiating the chat process");
        var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
        };

        var chatHistory = await chatHistoryDataService.GetChatHistoryByUser(userConnectionId);

        chatHistory.ChatHistory.AddUserMessage(userMessage);

        var template = """
            <message role="system">
            # System Instructions:
            You are a kind, intelligent health care chat bot, designed to answe user questions on various health topics.
            You have a very loving tone in your respones. 
            You will be greeting the user if its his or her message followed by answer to his or her query.
            You will be determining the user intent by using {{UserIntentPlugin-get_user_intent userMessage history}}. Based on the intent you will be using one of the matching plugin functions for responding to the user.
            Based on the recognized user intent you will use registered plugins to answer the questions.   
            </message>
            <message role="user">
            {{userMessage}}
            </message>
                {% for item in history %}
            <message role="{{item.role}}">
                {{item.content}}
            </message>
            """;

        var templateFactory = new HandlebarsPromptTemplateFactory();
        var promptTemplateConfig = new PromptTemplateConfig()
        {
            Template = template,
            TemplateFormat = "handlebars",
            Name = "ContosoChatPrompt",
        };
        var summary = await _kernel.InvokeAsync(
            "ConversationSummaryPlugin",
            "SummarizeConversation",
            new() { { "input", chatHistory.ChatHistory.ToString() } }
        );
        var arguments = new KernelArguments()
        {
            { "userMessage", userMessage },
            { "history", summary.ToString() },
        };
        // Render the prompt
        var promptTemplate = templateFactory.Create(promptTemplateConfig);
        var renderedPrompt = await promptTemplate.RenderAsync(_kernel, arguments);
        chatHistory.ChatHistory.AddSystemMessage(renderedPrompt);

        var chatMessage = await chatCompletionService.GetChatMessageContentAsync(
            chatHistory.ChatHistory,
            openAIPromptExecutionSettings,
            kernel: _kernel
        );
        logger.LogInformation("Response > {chatMessage}", chatMessage);
        chatHistory.ChatHistory.AddAssistantMessage(chatMessage.Content ?? string.Empty);

        //await chatHistoryDataService.SaveChatHistory(userConnectionId, chatHistory.ChatHistory);

        return chatMessage.Content ?? string.Empty;
    }

    public async Task<string> RunMedicalProviderSearchAsync(string zipcode)
    {
        logger.LogInformation("Initiating the chat process without user connection ID");
        var result = await _kernel.InvokeAsync(
            "MedicalProviderDatabasePlugin",
            "search_providers",
            new KernelArguments() { { "zipcode", zipcode }, { "kernel", _kernel } }
        );

        return result.ToString() ?? string.Empty;
    }
}
