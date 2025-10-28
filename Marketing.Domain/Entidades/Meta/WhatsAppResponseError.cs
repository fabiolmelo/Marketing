using System.Text.Json.Serialization;

namespace Marketing.Domain.Entidades.Meta
{
    public class WhatsAppResponseError
    {
        public Error error { get; set; } = new Error();
    }

    public class Error
    {
        [JsonPropertyName("message")]
        public string? MessageError { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("code")]
        public int? Code { get; set; }

        [JsonPropertyName("error_data")]
        public ErrorData? ErrorData { get; set; }

        [JsonPropertyName("error_subcode")]
        public int? ErrorSubcode { get; set; }

        [JsonPropertyName("fbtrace_id")]
        public string? FbtraceId { get; set; }
    }

    public class ErrorData
    {
        [JsonPropertyName("messaging_product")]
        public string? MessagingProduct { get; set; }
    }
}

