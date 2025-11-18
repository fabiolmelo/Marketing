namespace Marketing.Domain.Entidades
{
    public class Contato
    {
        public string Telefone { get; set; } = null!;
        public string? Nome { get; set; } = null!;
        public int AceitaMensagem {get; set; } = 0;
        public DateTime? DataAceite { get; set; }
        public int RecusaMensagem {get; set; } = 0;
        public DateTime? DataRecusa { get; set; }
        public string Email { get; set; } = String.Empty;
        public Guid Token { get; set; } = Guid.NewGuid();
        public DateTime? UltimaCompetenciaEnviada { get; set; }
        public ICollection<ContatoEstabelecimento> ContatoEstabelecimentos { get; set; } = null!;
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