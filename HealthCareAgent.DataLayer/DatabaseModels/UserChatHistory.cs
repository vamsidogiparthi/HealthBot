namespace HealthCareAgent.DataLayer.DatabaseModels;

using Microsoft.SemanticKernel.ChatCompletion;

public class UserChatHistory
{
    public string UserChatConnectId { get; set; } = string.Empty;
    public ChatHistory ChatHistory { get; set; } = [];
}
