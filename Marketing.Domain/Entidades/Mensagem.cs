namespace Marketing.Domain.Entidades
{
    public class Mensagem
    {
        public Mensagem()
        {
        }
        
        public Mensagem(string id)
        {
            Id = id;
        }

        public string Id { get; set; } = null!;
        public virtual EnvioMensagemMensal? EnvioMensagemMensal { get; set; }
        public ICollection<MensagemItem> MensagemItems { get; private set; } = null!;

        public void AdicionarEvento(MensagemItem evento)
        {
            this.MensagemItems.Add(evento);
        }

        public void AdicionarEvento(MensagemStatus mensagemStatus)
        {
            var evento = new MensagemItem();
            evento.MensagemId = this.Id;
            evento.Mensagem = this;
            evento.DataEvento = DateTime.Now;
            evento.MensagemStatus = mensagemStatus;  
            this.MensagemItems.Add(evento);
        }
    }
}