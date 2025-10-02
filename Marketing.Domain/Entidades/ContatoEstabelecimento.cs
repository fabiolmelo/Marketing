namespace Marketing.Domain.Entidades
{
    public class ContatoEstabelecimento
    {
        public ContatoEstabelecimento()
        {

        }

        public ContatoEstabelecimento(Contato contato, Estabelecimento estabelecimento)
        {
            Contato = contato;
            Estabelecimento = estabelecimento;
        }

        public ContatoEstabelecimento(string contatoTelefone, string estabelecimentoCnpj)
        {
            ContatoTelefone = contatoTelefone;
            EstabelecimentoCnpj = estabelecimentoCnpj;
        }

        public string ContatoTelefone { get; set; } = null!;
        public string EstabelecimentoCnpj { get; set; } = null!;
        public virtual Contato Contato { get; set; } = null!;
        public virtual Estabelecimento Estabelecimento { get; set; } = null!;
    }
}