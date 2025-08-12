using System.Text.Json.Serialization;

namespace Marketing.Domain.Entidades
{
    public class WhatsAppWebhookPayload
    {
        [JsonPropertyName("object")]
        public string? Object { get; set; } = String.Empty; 

        [JsonPropertyName("entry")]
        public List<Entry> Entry { get; set; } = new List<Entry>();
    }

    public class Entry
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; } = String.Empty;

        [JsonPropertyName("changes")]
        public List<Change> Changes { get; set; } = new List<Change>();
    }

    public class Change
    {
        [JsonPropertyName("field")]
        public string? Field { get; set; } = String.Empty;

        [JsonPropertyName("value")]
        public Value Value { get; set; } = new Value();
    }

    public class Value
    {
        [JsonPropertyName("messaging")]
        public Messaging Messaging { get; set; } = new Messaging();

        [JsonPropertyName("statuses")]
        public Statuses Statuses { get; set; } = new Statuses();
    }

    public class Messaging
    {
        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; } = new List<Message>();
    }

    public class Message
    {
        [JsonPropertyName("from")]
        public string? From { get; set; } = String.Empty;

        [JsonPropertyName("id")]
        public string? Id { get; set; } = String.Empty;

        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; } = String.Empty;

        [JsonPropertyName("text")]
        public Text Text { get; set; } = new Text();
    }

    public class Text
    {
        [JsonPropertyName("body")]
        public string? Body { get; set; } = String.Empty;
    }

    public class Statuses
    {
        [JsonPropertyName("statuses")]
        public List<Status> StatusesList { get; set; } = new List<Status>();
    }

    public class Status
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; } = String.Empty;

        [JsonPropertyName("status")]
        public string? StatusName { get; set; } = String.Empty;

        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; } = String.Empty;

        [JsonPropertyName("recipient_id")]
        public string? RecipientId { get; set; } = String.Empty;
    }
}