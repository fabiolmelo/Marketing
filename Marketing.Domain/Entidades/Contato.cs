using System;
using System.Collections.Generic;

namespace Marketing.Domain.Entidades
{
    public class Contato
    {
        public string? Telefone { get; set; } = String.Empty;
        public string? Nome { get; set; } = String.Empty;
        public bool AceitaMensagem {get; set; }
        public DateTime? DataAceite { get; set; }
        public bool RecusaMensagem {get; set; }
        public DateTime? DataRecusa { get; set; }
        public string? Email { get; set; } = String.Empty;
        public Guid Token { get; set; }
        public DateTime UltimaCompetenciaEnviada { get; set; }
        public ICollection<Estabelecimento> Estabelecimentos { get; set; } = new List<Estabelecimento>();
        public Contato()
        {
            Token = Guid.NewGuid();
            Estabelecimentos = new List<Estabelecimento>();
        }
        public Contato(string telefone)
        {
            Telefone = telefone;
            Token = Guid.NewGuid();
            Estabelecimentos = new List<Estabelecimento>();
        }
    }
}