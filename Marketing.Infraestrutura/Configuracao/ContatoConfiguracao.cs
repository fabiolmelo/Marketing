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
            builder.Property(x => x.Telefone)
                   .HasMaxLength(15)
                   .IsRequired();

            builder.Property(x => x.Nome).HasMaxLength(120);

            builder.Property(x => x.AceitaMensagem).HasColumnType("TINYINT(1)");
            builder.Property(x => x.DataAceite).HasColumnType("DATETIME");

            builder.Property(x => x.RecusaMensagem).HasColumnType("TINYINT(1)");
            builder.Property(x => x.DataRecusa).HasColumnType("DATETIME");

            builder.Property(x => x.Email).HasMaxLength(120);

            builder.Property(x => x.Token).HasMaxLength(80);
            builder.HasIndex(x => x.Token).HasDatabaseName("IX_CONTATO_TOKEN");

            builder.Property(x => x.UltimaCompetenciaEnviada).HasColumnType("DATETIME");

            builder.Ignore( x=> x.ContatoEstabelecimentos);

            builder.HasMany(x => x.ContatoEstabelecimentos)
                   .WithOne(x => x.Contato)
                   .HasForeignKey(x => x.ContatoTelefone);
        }
    }
}