namespace Marketing.Domain.Entidades
{
    public class MensagemEvento
    {
        public MensagemEvento()
        {
        }
        public MensagemEvento(string mensagemId, MensagemStatus mensagemStatus)
        {
            MensagemId = mensagemId;
            MensagemStatus = mensagemStatus;
        }

        public int? Id { get; set; }
        public string? MensagemId { get; set; }
        public virtual Mensagem? Mensagem { get; set; }
        public MensagemStatus? MensagemStatus { get; set; }
        public DateTime DataEvento { get; set; } = DateTime.Now;
        
    }
}