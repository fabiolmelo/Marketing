namespace Marketing.Domain.Entidades
{
    public class ContatoEstabelecimento
    {
        public ContatoEstabelecimento()
        {

        }

        public ContatoEstabelecimento(string contatoTelefone, string estabelecimentoCnpj, string estabelecimentoRedeNome)
        {
            ContatoTelefone = contatoTelefone;
            EstabelecimentoCnpj = estabelecimentoCnpj;
            EstabelecimentoRedeNome = estabelecimentoRedeNome;
        }

        public string ContatoTelefone { get; set; } = null!;
        public string EstabelecimentoCnpj { get; set; } = null!;
        public string EstabelecimentoRedeNome { get; set; } = null!;
        public virtual Contato Contato { get; set; } = null!;
        public virtual Estabelecimento Estabelecimento { get; set; } = null!;
    }
}