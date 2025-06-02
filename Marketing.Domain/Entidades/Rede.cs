namespace Marketing.Domain.Entidades
{
    public class Rede
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; } = DateTime.Now;
        public virtual ICollection<Estabelecimento> Estabelecimentos { get; set; } = new List<Estabelecimento>();
        public Rede(string nome)
        {
            Nome = nome;
        }
    }
}