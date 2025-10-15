namespace Marketing.Domain.Entidades
{
    public class Rede
    {
        public string Nome { get; set; }
        public DateTime DataCadastro { get; private set; } = DateTime.Now;
        public string Logo { get; set; } = $"{Guid.NewGuid().ToString()}.png";
        public virtual List<Estabelecimento> Estabelecimentos { get; set; } = new List<Estabelecimento>();
        public Rede(string nome)
        {
            Nome = nome;
        }
    }
}