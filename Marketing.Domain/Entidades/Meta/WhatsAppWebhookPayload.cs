using System.Text.Json.Serialization;

namespace Marketing.Domain.Entidades.Meta
{
    public class WhatsAppWebhookPayload
    {
        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("entry")]
        public List<Entry> Entry { get; set; } = new List<Entry>();
    }

    public class Entry
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("changes")]
        public List<Change> Changes { get; set; } = new List<Change>();
    }

    public class Change
    {
        [JsonPropertyName("value")]
        public ChangeValue Value { get; set; } = new ChangeValue();

        [JsonPropertyName("field")]
        public string? Field { get; set; }
    }

    public class ChangeValue
    {
        [JsonPropertyName("messaging_product")]
        public string? MessagingProduct { get; set; }

        [JsonPropertyName("statuses")]
        public List<MessageStatus> Statuses { get; set; } = new List<MessageStatus>();
    }

    public class MessageStatus
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; }

        [JsonPropertyName("recipient_id")]
        public string? RecipientId { get; set; }

        [JsonPropertyName("conversation")]
        public Conversation? Conversation { get; set; }

        [JsonPropertyName("pricing")]
        public Pricing? Pricing { get; set; }

        [JsonPropertyName("errors")]
        public List<StatusError>? Errors { get; set; }
    }

    public class Conversation
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("origin")]
        public ConversationOrigin? Origin { get; set; }
    }

    public class ConversationOrigin
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }

    public class Pricing
    {
        [JsonPropertyName("pricing_model")]
        public string? PricingModel { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }
    }

    public class StatusError
    {
        [JsonPropertyName("code")]
        public int? Code { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
