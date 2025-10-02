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

        public DateTime Competencia { get; set; }
        public string EstabelecimentoCnpj { get; set; } = String.Empty;
        public string ContatoTelefone { get; set; } = String.Empty;
        public string MensagemId { get; set; } = String.Empty;
        public virtual Mensagem Mensagem { get; set; } = new Mensagem();
    }
}