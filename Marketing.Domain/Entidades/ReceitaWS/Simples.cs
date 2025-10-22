using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Marketing.Domain.Entidades.ReceitaWS
{
    public class Simples
    {
        [JsonPropertyName("optante")]
        public bool Optante { get; set; }

        [JsonPropertyName("data_opcao")]
        public string? DataOpcao { get; set; }

        [JsonPropertyName("data_exclusao")]
        public string? DataExclusao { get; set; }

        [JsonPropertyName("ultima_atualizacao")]
        public DateTime? UltimaAtualizacao { get; set; }
    }
}