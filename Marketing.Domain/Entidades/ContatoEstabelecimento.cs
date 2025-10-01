namespace Marketing.Domain.Entidades
{
    public class ContatoEstabelecimento
    {
        public ContatoEstabelecimento()
        {

        }
        public string ContatoTelefone { get; set; } = null!;
        public virtual Contato Contato { get; set; } = null!;
        public string EstabelecimentoCnpj { get; set; } = null!;
        public virtual Estabelecimento Estabelecimento { get; set; } = null!;
    }
}