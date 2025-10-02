using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class MensagemItemConfiguracao : IEntityTypeConfiguration<MensagemItem>
    {
        public void Configure(EntityTypeBuilder<MensagemItem> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}