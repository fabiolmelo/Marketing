using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class FechamentoMensalConfiguracao : IEntityTypeConfiguration<FechamentoMensal>
    {
        public void Configure(EntityTypeBuilder<FechamentoMensal> builder)
        {
            builder.HasKey(x => new { x.Competencia, x.EstabelecimentoCnpj });
            builder.Property(x => x.Competencia).HasColumnType("DATE");
        }
    }
}
