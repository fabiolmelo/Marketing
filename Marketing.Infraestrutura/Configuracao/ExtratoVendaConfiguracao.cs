using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class ExtratoVendaConfiguracao : IEntityTypeConfiguration<ExtratoVendas>
    {
        public void Configure(EntityTypeBuilder<ExtratoVendas> builder)
        {
            builder.HasKey(x => new { x.Ano, x.Mes, x.EstabelecimentoCnpj });
            builder.Property(x => x.IncidenciaReal).HasColumnType("NUMERIC");
            builder.Property(x => x.IncidenciaReal).HasPrecision(8, 4);
            builder.Property(x => x.Meta).HasColumnType("NUMERIC");
            builder.Property(x => x.Meta).HasPrecision(18, 4);
            builder.Property(x => x.PrecoUnitarioMedio).HasColumnType("NUMERIC");
            builder.Property(x => x.PrecoUnitarioMedio).HasPrecision(8, 2);
            builder.Property(x => x.ReceitaNaoCapturada).HasColumnType("NUMERIC");
            builder.Property(x => x.ReceitaNaoCapturada).HasPrecision(8, 2);
            builder.Property(x => x.CorTransparenteGrafico).HasColumnType("NUMERIC");
            builder.Property(x => x.CorTransparenteGrafico).HasPrecision(8, 2);
            builder.Property(x => x.CorVerdeGrafico).HasColumnType("NUMERIC"); ;
            builder.Property(x => x.CorVerdeGrafico).HasPrecision(8, 2);
            builder.Property(x => x.CorVermelhaGrafico).HasColumnType("NUMERIC"); 
            builder.Property(x => x.CorVermelhaGrafico).HasPrecision(8, 2);
        }
    }
}