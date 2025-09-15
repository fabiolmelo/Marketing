namespace Marketing.Domain.Entidades
{
    public class Mensagem
    {
        public Mensagem()
        {
            IdMessage = String.Empty;
            ContatoTelefone = String.Empty;
            Contato = new Contato();
        }

        public Mensagem(string idMessage, string contatoTelefone, Contato contato)
        {
            IdMessage = idMessage;
            ContatoTelefone = contatoTelefone;
            Contato = contato;
        }

        public string IdMessage { get; set; } 
        public string ContatoTelefone { get; set; }
        public virtual Contato Contato { get; set; }
       
    }
}