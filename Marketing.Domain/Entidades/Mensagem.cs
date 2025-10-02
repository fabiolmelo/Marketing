namespace Marketing.Domain.Entidades
{
    public class Mensagem
    {
        public Mensagem()
        {
            MensagemItems = MensagemItems ?? new List<MensagemItem>();
        }
        public Mensagem(string id)
        {
            Id = id;
            MensagemItems = MensagemItems ?? new List<MensagemItem>();
        }

        public string Id { get; set; } = null!;
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