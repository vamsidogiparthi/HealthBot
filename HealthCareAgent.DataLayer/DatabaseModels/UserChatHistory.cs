namespace HealthCareAgent.DataLayer.DatabaseModels;

using Microsoft.SemanticKernel.ChatCompletion;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class UserChatHistory
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string UserChatConnectId { get; set; } = string.Empty;

    [BsonElement("chatHistory")]
    public ChatHistory ChatHistory { get; set; } = [];
}
