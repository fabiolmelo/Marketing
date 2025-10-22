using System.Text.Json.Serialization;

namespace Marketing.Domain.Entidades.ReceitaWS
{
    public class Qsa
    {
        [JsonPropertyName("nome")]
        public string? Nome { get; set; }

        [JsonPropertyName("qual")]
        public string? Qual { get; set; }
    }
}