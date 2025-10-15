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
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<ContatoEstabelecimento> ContatoEstabelecimento { get; set; }
        public DbSet<Rede> Redes { get; set; }
        public DbSet<ImportacaoEfetuada> ImportacaoEfetuada { get; set; }
        public DbSet<DadosPlanilha> DadosPlanilha { get; set; }
        public DbSet<EnvioMensagemMensal> EnviosMensagemMensais { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }
        public DbSet<MensagemItem> MensagemItems { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                // var connectionString = "Data Source=DadosApp\\BD\\LocalDatabase.db";
                // var serverVersion = new MySqlServerVersion(new Version(10, 2));
                // var connectionString = "Server=mysql.mediaonboard.com.br;Database=mediaonboard02;Uid=mediaonboard02;Pwd=Hh8bjdT2Mh82K8u;";
                // optionsBuilder.UseMySql(connectionString, serverVersion)
                //                         .LogTo(Console.WriteLine, LogLevel.Information)
                //                         .EnableSensitiveDataLogging()
                //                         .EnableDetailedErrors();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ImportacaoEfetuadaConfiguracao());
            modelBuilder.ApplyConfiguration(new ExtratoVendaConfiguracao());
            modelBuilder.ApplyConfiguration(new EstabelecimentoConfiguracao());
            modelBuilder.ApplyConfiguration(new ContatoConfiguracao());
            modelBuilder.ApplyConfiguration(new RedeConfiguracao());
            modelBuilder.ApplyConfiguration(new DadosPlanilhaConfiguration());
            modelBuilder.ApplyConfiguration(new EnvioMensagemMensalConfiguracao());
            modelBuilder.ApplyConfiguration(new ContatoEstabelecimentoConfiguracao());
            modelBuilder.ApplyConfiguration(new MensagemConfiguracao());
            modelBuilder.ApplyConfiguration(new MensagemItemConfiguracao());
        }
    }
}