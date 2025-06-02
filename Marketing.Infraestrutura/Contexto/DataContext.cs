using Marketing.Domain.Entidades;
using Marketing.Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Contexto
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<ExtratoVendas> ExtratosVendas { get; set; }
        public DbSet<ImportacaoEfetuada> ImportacoesEfetuadas { get; set; }
        public DbSet<Estabelecimento> Estabelecimentos { get; set; }
        public DbSet<EstabelecimentoContato> EstabelecimentoContatos { get; set; }
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Rede> Redes { get; set; }
        public DbSet<ImportacaoEfetuada> ImportacaoEfetuada { get; set; }
        public DbSet<DadosPlanilha> DadosPlanilha { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Data Source=DadosApp\\BD\\LocalDatabase.db";
            optionsBuilder.UseSqlite(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ImportacaoEfetuadaConfiguracao());
            modelBuilder.ApplyConfiguration(new ExtratoVendaConfiguracao());
            modelBuilder.ApplyConfiguration(new EstabelecimentoConfiguracao());
            modelBuilder.ApplyConfiguration(new ContatoConfiguracao());
            modelBuilder.ApplyConfiguration(new EstabelecimentoContatoConfiguracao());
            modelBuilder.ApplyConfiguration(new RedeConfiguracao());
            modelBuilder.ApplyConfiguration(new DadosPlanilhaConfiguration());
        }
    }
}