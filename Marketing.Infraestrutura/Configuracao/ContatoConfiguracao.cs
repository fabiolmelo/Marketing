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
            builder.Property(x => x.Telefone).HasMaxLength(15);

            builder.Property(x => x.Token).HasMaxLength(80);
            builder.HasIndex(x => x.Token).HasDatabaseName("IX_CONTATO_TOKEN");

            builder.HasMany(x => x.ContatoEstabelecimentos)
                   .WithOne(x => x.Contato)
                   .HasForeignKey(x => x.ContatoTelefone);
        }
    }
}