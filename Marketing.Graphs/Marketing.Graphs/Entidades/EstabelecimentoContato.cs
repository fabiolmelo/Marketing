namespace Marketing.Graphs.Entidades
{ 
    public class EstabelecimentoContato
    {
        public EstabelecimentoContato(string estabelecimentoCnpj, string contatoTelefone)
        {
            EstabelecimentoCnpj = estabelecimentoCnpj;
            ContatoTelefone = contatoTelefone;
        }

        public string EstabelecimentoCnpj { get; set; }
        public string ContatoTelefone { get; set; }

        public virtual Contato Contato { get; set; } = new Contato();
        public virtual Estabelecimento Estabelecimento { get; set; } = new Estabelecimento();
    }
}