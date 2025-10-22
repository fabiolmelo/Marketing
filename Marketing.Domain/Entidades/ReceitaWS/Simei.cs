using System.Text.Json.Serialization;

namespace Marketing.Domain.Entidades.ReceitaWS
{
    public class Simei
    {
        [JsonPropertyName("optante")]
        public bool Optante { get; set; }

        [JsonPropertyName("data_opcao")]
        public object? DataOpcao { get; set; }

        [JsonPropertyName("data_exclusao")]
        public object? DataExclusao { get; set; }

        [JsonPropertyName("ultima_atualizacao")]
        public DateTime? UltimaAtualizacao { get; set; }
    }
}