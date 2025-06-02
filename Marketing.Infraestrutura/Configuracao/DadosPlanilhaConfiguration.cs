using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class DadosPlanilhaConfiguration : IEntityTypeConfiguration<DadosPlanilha>
    {
        public void Configure(EntityTypeBuilder<DadosPlanilha> builder)
        {
            builder.HasKey(x => new { x.ImportacaoEfetuadaId, x.Cnpj, x.AnoMes});
        }
    }
}