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

            builder.HasMany(x => x.MensagemItems)
                   .WithOne(x => x.Mensagem)
                   .HasForeignKey(x => x.MensagemId);
        }
    }
}