namespace Marketing.Domain.Entidades
{
    public class Mensagem
    {
        public Mensagem(string id, string contatoFone, DateTime competencia)
        {
            Id = id;
            ContatoFone = contatoFone;
            Competencia = competencia;
        }

        public string Id { get; set; }
        public string ContatoFone { get; set; }
        public virtual Contato Contato { get; set; } = new Contato();
        public DateTime Competencia { get; set; }
        public List<MensagemEvento> MensagemEvento { get; private set; } = new List<MensagemEvento>();

        public void AdicionarEvento(MensagemStatus mensagemStatus) {
            MensagemEvento.Add(new MensagemEvento(this.Id, mensagemStatus));
        }
    }   
}