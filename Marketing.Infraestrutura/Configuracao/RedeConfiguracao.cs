using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class RedeConfiguracao : IEntityTypeConfiguration<Rede>
    {
        public void Configure(EntityTypeBuilder<Rede> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.Property(x=>x.Nome).HasMaxLength(50);
        }
    }
}