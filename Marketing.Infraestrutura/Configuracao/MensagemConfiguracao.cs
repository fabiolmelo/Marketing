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
            builder.Property(x => x.ContatoTelefone).HasMaxLength(20);

            builder.HasMany(me => me.MensagemEvento)
                   .WithOne(m => m.Mensagem)
                   .HasForeignKey(x => x.MensagemId);
        }
    }
}