using Marketing.Domain.Entidades;
using Marketing.Infraestrutura.Configuracao;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Contexto
{
    public class DataContext : IdentityDbContext<UsuarioEntity>
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
        public DbSet<ConfiguracaoApp> Configuracoes { get; set; }
        public DbSet<TemplateImportarPlanilha> TemplateImportarPlanilhas { get; set; }
        public DbSet<MetaWebhookResponse> MetaWebhookResponses { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
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

            modelBuilder.ApplyConfiguration(new IdentityUserConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityRoleConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityUserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityUserClaimConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityRoleClaimConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityUserLoginConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityUserTokenConfiguration());

        }
    }

    public class UsuarioEntity : IdentityUser
    {
        public string? Nome { get; set; }
    }
}