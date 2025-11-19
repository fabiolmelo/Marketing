using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class MensagemConfiguracao : IEntityTypeConfiguration<Mensagem>
    {
        public void Configure(EntityTypeBuilder<Mensagem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.MetaMensagemId).HasMaxLength(200);
            builder.HasIndex(x => x.MetaMensagemId).HasDatabaseName("IX_MENSAGEM_METAMENSAGEMID");

            builder.HasMany(x => x.MensagemItems)
                   .WithOne(x => x.Mensagem)
                   .HasForeignKey(x => x.MensagemId);
        }
    }
}