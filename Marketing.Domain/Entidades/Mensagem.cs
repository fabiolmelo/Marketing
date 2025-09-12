namespace Marketing.Domain.Entidades
{
    public class Mensagem
    {
        public Mensagem()
        {
        }
        public Mensagem(string id, string contatoFone, DateTime competencia)
        {
            Id = id;
            ContatoTelefone = contatoFone;
            Competencia = competencia;
        }

        public string Id { get; set; } = string.Empty; 
        public string ContatoTelefone { get; set; } = string.Empty;
        public DateTime Competencia { get; set; }
        public ICollection<MensagemEvento> MensagemEvento { get; private set; } = new List<MensagemEvento>();

        public void AdicionarEvento(MensagemStatus mensagemStatus) {
            MensagemEvento.Add(new MensagemEvento(this.Id, mensagemStatus));
        }
    }   
}