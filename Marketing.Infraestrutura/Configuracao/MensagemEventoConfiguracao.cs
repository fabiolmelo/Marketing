using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class MensagemEventoConfiguracao : IEntityTypeConfiguration<MensagemEvento>
    {
        public void Configure(EntityTypeBuilder<MensagemEvento> builder)
        {
            builder.HasKey(x => new { x.MensagemId, x.MensagemStatus });
        }
    }
}