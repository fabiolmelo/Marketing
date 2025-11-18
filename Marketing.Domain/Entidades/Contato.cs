namespace Marketing.Domain.Entidades
{
    public class Contato
    {
        public string Telefone { get; set; } = null!;
        public string? Nome { get; set; } 
        public bool AceitaMensagem {get; set; } 
        public DateTime? DataAceite { get; set; }
        public bool RecusaMensagem {get; set; } 
        public DateTime? DataRecusa { get; set; }
        public string? Email { get; set; }
        public Guid Token { get; set; } = Guid.NewGuid();
        public DateTime? UltimaCompetenciaEnviada { get; set; }
        public virtual ICollection<ContatoEstabelecimento> ContatoEstabelecimentos { get; set; } = null!;
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public OrigemContato? OrigemContato { get; set; }
        
        public Contato()
        {
        }
        public Contato(string telefone)
        {
            Telefone = telefone;
        }
    }
}