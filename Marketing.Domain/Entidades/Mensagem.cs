namespace Marketing.Domain.Entidades
{
    public class Mensagem
    {
        public Mensagem(string id, string contatoFone, DateTime competencia)
        {
            Id = id;
            ContatoTelefone = contatoFone;
            Competencia = competencia;
        }

        public string Id { get; set; }
        public string ContatoTelefone { get; set; }
        public DateTime Competencia { get; set; }
        public ICollection<MensagemEvento> MensagemEvento { get; private set; } = new List<MensagemEvento>();

        public void AdicionarEvento(MensagemStatus mensagemStatus) {
            MensagemEvento.Add(new MensagemEvento(this.Id, mensagemStatus));
        }
    }   
}