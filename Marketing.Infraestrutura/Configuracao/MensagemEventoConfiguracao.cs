using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class MensagemEventoConfiguracao : IEntityTypeConfiguration<MensagemEvento>
    {
        public void Configure(EntityTypeBuilder<MensagemEvento> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            //builder.Property(x => x.MensagemStatus).HasConversion<int>();

                  
            builder.HasOne(x=>x.Mensagem).
                    WithMany(x=>x.MensagemEvento).
                    HasPrincipalKey(x=>x.Id);
        }
    }
}