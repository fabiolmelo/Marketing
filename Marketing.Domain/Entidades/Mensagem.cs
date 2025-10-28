namespace Marketing.Domain.Entidades
{
    public class Mensagem
    {
        public Mensagem()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Mensagem(string id)
        {
            Id = id;
        }

        public string Id { get; set; } 
        public virtual EnvioMensagemMensal? EnvioMensagemMensal { get; set; }
        public ICollection<MensagemItem> MensagemItems { get; private set; } = null!;

        public void AdicionarEvento(MensagemItem evento)
        {
            if (this.MensagemItems == null) this.MensagemItems = new List<MensagemItem>();
            this.MensagemItems.Add(evento);
        }

        public void AdicionarEvento(MensagemStatus mensagemStatus)
        {
            if (this.MensagemItems == null) this.MensagemItems = new List<MensagemItem>();
            var evento = new MensagemItem();
            evento.MensagemId = this.Id;
            evento.Mensagem = this;
            evento.DataEvento = DateTime.Now;
            evento.MensagemStatus = mensagemStatus;
            this.MensagemItems.Add(evento);
        }
          public void AdicionarEvento(MensagemStatus mensagemStatus, string observacao)
        {
            if (this.MensagemItems == null) this.MensagemItems = new List<MensagemItem>();
            var evento = new MensagemItem();
            evento.MensagemId = this.Id;
            evento.Mensagem = this;
            evento.DataEvento = DateTime.Now;
            evento.MensagemStatus = mensagemStatus;
            evento.Observacao = observacao; 
            this.MensagemItems.Add(evento);
        }
    }
}