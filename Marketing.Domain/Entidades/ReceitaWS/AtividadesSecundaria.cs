using System.Text.Json.Serialization;

namespace Marketing.Domain.Entidades.ReceitaWS
{
    public class AtividadesSecundaria
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}