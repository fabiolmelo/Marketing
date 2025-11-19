namespace Marketing.Domain.Entidades
{
    public class Mensagem
    {
      
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public virtual EnvioMensagemMensal EnvioMensagemMensal { get; set; } = null!;
        public string? MetaMensagemId { get; private set; }
        public virtual ICollection<MensagemItem> MensagemItems { get; private set; } = new List<MensagemItem>();

        public void SetMetaMensagemId(string id)
        {
            MetaMensagemId = id;
        }
        public void AdicionarEvento(MensagemItem evento)
        {
            evento.MensagemId = this.Id;
            MensagemItems.Add(evento);
        }

        public void AdicionarEvento(MensagemStatus mensagemStatus)
        {
            MensagemItems.Add(new MensagemItem(this.Id, DateTime.Now, mensagemStatus));
        }
        public void AdicionarEvento(MensagemStatus mensagemStatus, string observacao)
        {
            MensagemItems.Add(new MensagemItem(this.Id, DateTime.Now, mensagemStatus, observacao));
        }
    }
}