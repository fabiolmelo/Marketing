using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class MensagemEnviadaConfiguracao : IEntityTypeConfiguration<MensagemEnviada>
    {
        public void Configure(EntityTypeBuilder<MensagemEnviada> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("VARCHAR(5000)");
        }
    }
}