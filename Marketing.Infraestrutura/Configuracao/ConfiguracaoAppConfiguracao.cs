using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class ConfiguracaoAppConfiguracao : IEntityTypeConfiguration<ConfiguracaoApp>
    {
        public void Configure(EntityTypeBuilder<ConfiguracaoApp> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}