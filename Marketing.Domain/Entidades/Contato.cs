namespace Marketing.Domain.Entidades
{
    public class Contato
    {
        public string Telefone { get; set; } = null!;
        public string? Nome { get; set; } = null!;
        public bool AceitaMensagem {get; set; }
        public DateTime? DataAceite { get; set; }
        public bool RecusaMensagem {get; set; }
        public DateTime? DataRecusa { get; set; }
        public string? Email { get; set; } = String.Empty;
        public Guid Token { get; set; }
        public DateTime? UltimaCompetenciaEnviada { get; set; }
        public ICollection<ContatoEstabelecimento> ContatoEstabelecimentos { get; set; } = null!;
        
        public Contato()
        {
            Token = Guid.NewGuid();
        }
        public Contato(string telefone)
        {
            Telefone = telefone;
            Token = Guid.NewGuid();
        }
    }
}