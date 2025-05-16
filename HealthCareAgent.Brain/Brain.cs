using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace HealthCareAgent.Brain;

public interface IBrain
{
    Task RunAsync();
}

public class Brain([FromKeyedServices("ChatBotKernel")] Kernel kernel, ILogger<Brain> logger)
    : IBrain
{
    private readonly Kernel _kernel = kernel;
    private readonly ILogger<Brain> _logger = logger;

    public async Task RunAsync()
    {
        _logger.LogInformation("Initiating the chat process");
        var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
        };

        var chatHistory = new ChatHistory();
        var chatMessage = await chatCompletionService.GetChatMessageContentAsync(
            chatHistory,
            openAIPromptExecutionSettings
        );
    }
}
