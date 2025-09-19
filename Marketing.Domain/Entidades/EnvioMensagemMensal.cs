namespace Marketing.Domain.Entidades
{
    public class EnvioMensagemMensal
    {
        public EnvioMensagemMensal()
        {
        }

        public EnvioMensagemMensal(DateTime competencia, string estabelecimentoCnpj,
                                   Estabelecimento estabelecimento, string contatoTelefone, Contato contato)
        {
            Competencia = competencia;
            EstabelecimentoCnpj = estabelecimentoCnpj;
            Estabelecimento = estabelecimento;
            ContatoTelefone = contatoTelefone;
            Contato = contato;
        }

        public DateTime Competencia { get; set; }
        public string? EstabelecimentoCnpj { get; set; }
        public virtual Estabelecimento? Estabelecimento { get; set; } 
        public string? ContatoTelefone { get; set; }
        public virtual Contato? Contato { get; set; } 
        public string? MensagemId { get; set; }
        public virtual Mensagem? Mensagem { get; set; }
    }
}