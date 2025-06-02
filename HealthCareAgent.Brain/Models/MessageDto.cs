namespace HealthCareAgent.Brain.Models;

public record MessageDto
{
    public string Sender { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public MessageDto(string sender, string recipient, string content, DateTime timestamp)
    {
        Sender = sender;
        Recipient = recipient;
        Content = content;
        Timestamp = timestamp;
    }
}
