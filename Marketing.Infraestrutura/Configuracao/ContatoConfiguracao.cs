using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class ContatoConfiguracao : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.HasKey(x => x.Telefone);
            builder.Property(x => x.Telefone).HasColumnType("VARCHAR(250)");

            builder.Property(x => x.Token).HasMaxLength(50);
            builder.HasIndex(x => x.Token).HasDatabaseName("IX_CONTATO_TOKEN");
        }
    }
}