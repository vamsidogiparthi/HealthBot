using HealthCareAgent.Brain;
using HealthCareAgent.Brain.Models;
using Microsoft.AspNetCore.SignalR;

namespace HealthCareAgent.WebAPI.Hubs;

public interface IChatClient
{
    Task ReceiveMessage(MessageDto messageDto);
    // Task SendMessageToCaller(string user, string message);
    // Task SendMessageToGroup(string user, string message);
}

public class ChatHub(ILogger<ChatHub> logger, IBrain brain) : Hub<IChatClient>
{
    public async Task SendMessage(MessageDto messageDto)
    {
        logger.LogInformation(
            "Sending message from {User}: {Message}",
            messageDto.Sender,
            messageDto.Content
        );
        var response = await brain.RunAsync(messageDto.Content, Context.ConnectionId);
        await Clients.All.ReceiveMessage(
            new MessageDto("Agent", "User", response, DateTime.UtcNow)
        );
    }

    public override async Task OnConnectedAsync()
    {
        logger.LogInformation("User connected: {ConnectionId}", Context.ConnectionId);
        // Add user to a group, etc. (if needed)
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Handle disconnection (e.g., remove from a group)
        logger.LogInformation("User disconnected: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    // public async Task SendMessageToCaller(string user, string message) =>
    //     await Clients.Caller.ReceiveMessage(user, message);

    // public async Task SendMessageToGroup(string user, string message) =>
    //     await Clients.Group("SignalR Users").ReceiveMessage(user, message);
}
