namespace Marketing.Domain.Entidades
{
    public class EnvioMensagemMensal
    {
        public EnvioMensagemMensal()
        {
        }

        public EnvioMensagemMensal(DateTime competencia)
        {
            Competencia = competencia;
        }

        public EnvioMensagemMensal(DateTime competencia, string estabelecimentoCnpj, string contatoTelefone)
        {
            Competencia = competencia;
            EstabelecimentoCnpj = estabelecimentoCnpj;
            ContatoTelefone = contatoTelefone;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Competencia { get; set; }
        public string EstabelecimentoCnpj { get; set; } = String.Empty;
        public string ContatoTelefone { get; set; } = String.Empty;
        public DateTime DataGeracao { get; set; } = DateTime.Now;
        public string? MensagemId { get; set; } = null!;
        public virtual Mensagem? Mensagem { get; set; } = null!;

    }
}