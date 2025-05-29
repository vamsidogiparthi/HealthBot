using HealthCareAgent.DataLayer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

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
    private readonly ILogger<Brain> _logger = logger;

    public async Task<string> RunAsync(string userMessage, string userConnectionId)
    {
        _logger.LogInformation("Initiating the chat process");
        var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
        };

        var chatHistory = await chatHistoryDataService.GetChatHistoryByUser(userConnectionId);

        chatHistory.ChatHistory.AddUserMessage(userMessage);
        var chatMessage = await chatCompletionService.GetChatMessageContentAsync(
            chatHistory.ChatHistory,
            openAIPromptExecutionSettings
        );
        _logger.LogInformation("Response > {chatMessage}", chatMessage);
        chatHistory.ChatHistory.AddAssistantMessage(chatMessage.Content ?? string.Empty);

        await chatHistoryDataService.SaveChatHistory(userConnectionId, chatHistory.ChatHistory);

        return chatMessage.Content ?? string.Empty;
    }

    public async Task<string> RunMedicalProviderSearchAsync(string zipcode)
    {
        _logger.LogInformation("Initiating the chat process without user connection ID");
        var result = await kernel.InvokeAsync(
            "MedicalProviderDatabasePlugin",
            "search_providers",
            new KernelArguments() { { "zipcode", zipcode }, { "kernel", kernel } }
        );

        return result.ToString() ?? string.Empty;
    }
}
