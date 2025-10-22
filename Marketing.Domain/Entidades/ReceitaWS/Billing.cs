using System.Text.Json.Serialization;

namespace Marketing.Domain.Entidades.ReceitaWS
{
    public class Billing
    {
        [JsonPropertyName("free")]
        public bool Free { get; set; }

        [JsonPropertyName("database")]
        public bool Database { get; set; }
    }
}