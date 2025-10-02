namespace Marketing.Domain.Entidades
{
    public class MensagemItem
    {
        public MensagemItem()
        {
        }
        public MensagemItem(string mensagemId, Mensagem mensagem, DateTime dataEvento, MensagemStatus mensagemStatus)
        {
            MensagemId = mensagemId;
            Mensagem = mensagem;
            DataEvento = dataEvento;
            MensagemStatus = mensagemStatus;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public string MensagemId { get; set; } = null!;
        public virtual Mensagem Mensagem { get; set; } = null!;
        public DateTime DataEvento { get; set; }
        public MensagemStatus MensagemStatus { get; set; }
        public string? Observacao { get; set; } 
    }
}