namespace HealthCareAgent.Brain.Plugins.FunctionPlugins;

public class SummaryPlugin
{
    [KernelFunction("summarize_conversation")]
    public async Task<FunctionResult?> SummarizeConversation(
        [Description("user chat history")] string chatHistory,
        Kernel kernel
    )
    {
        var summary = await kernel.InvokeAsync(
            "ConversationSummaryPlugin",
            "SummarizeConversation",
            new() { { "input", chatHistory.ToString() } }
        );
        return summary;
    }
}
