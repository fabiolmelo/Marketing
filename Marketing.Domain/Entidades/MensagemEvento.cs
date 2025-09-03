namespace Marketing.Domain.Entidades
{
    public class MensagemEvento
    {
        private MensagemEvento()
        {
        }
        public MensagemEvento(string mensagemId, MensagemStatus mensagemStatus)
        {
            MensagemId = mensagemId;
            MensagemStatus = mensagemStatus;
        }

        public string MensagemId { get; set; }
        public MensagemStatus? MensagemStatus { get; set; }
        public DateTime DataEvento { get; set; } = DateTime.Now;
        public virtual Mensagem? Mensagem { get; set; }
    }
}