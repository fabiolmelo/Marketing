using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class MensagemConfiguracao : IEntityTypeConfiguration<Mensagem>
    {
        public void Configure(EntityTypeBuilder<Mensagem> builder)
        {
            builder.HasKey(x => x.IdMessage);
            builder.Property(x => x.IdMessage).HasColumnType("VARCHAR").HasMaxLength(250);

            builder.HasOne(x => x.Contato).WithMany(x => x.Mensagens).HasPrincipalKey(x => x.Telefone); 
        }
    }
}