namespace HealthCareAgent.DataLayer.DatabaseModels;

using System.Text;
using Microsoft.SemanticKernel.ChatCompletion;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class UserChatHistory
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string UserChatConnectId { get; set; } = string.Empty;

    [BsonElement("chatHistory")]
    public History History { get; set; } = new();

    public UserChatHistory() { }

    public UserChatHistory(string userConnectionId)
    {
        UserChatConnectId = userConnectionId;
        History = new();
    }
}

public class History
{
    [BsonElement("messages")]
    public List<HistoryMessage> Messages { get; set; } = [];

    [BsonIgnore]
    public ChatHistory ChatHistory
    {
        get
        {
            ChatHistory messages = [];
            foreach (var msg in Messages)
            {
                messages.AddMessage(msg.Role, msg.Message);
            }

            return messages;
        }
    }

    public override string ToString()
    {
        StringBuilder result = new();
        foreach (var msg in Messages)
        {
            result.Append($"{msg.Role}: {msg.Message}");
        }

        return result.ToString();
    }
};

public record HistoryMessage(AuthorRole Role, string Message);
