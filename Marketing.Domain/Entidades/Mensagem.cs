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

        public string? Id { get; set; } 
        public ICollection<EnvioMensagemMensal> MensagensMensais { get; set; } = new List<EnvioMensagemMensal>();
    }
}