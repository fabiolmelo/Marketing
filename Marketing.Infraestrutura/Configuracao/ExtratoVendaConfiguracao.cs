using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class ExtratoVendaConfiguracao : IEntityTypeConfiguration<ExtratoVendas>
    {
        public void Configure(EntityTypeBuilder<ExtratoVendas> builder)
        {
            //builder.HasKey(x => new { x.Ano, x.Mes, x.EstabelecimentoCnpj });
            builder.HasKey(x => new { x.Competencia, x.EstabelecimentoCnpj });
            builder.HasOne(x=>x.Estabelecimento).WithMany(x=>x.ExtratoVendas).HasForeignKey(x=>x.EstabelecimentoCnpj);
            builder.Property(x => x.Competencia).HasColumnType("DATETIME");
            builder.Property(x => x.IncidenciaReal).HasColumnType("DECIMAL(18,2)");
            builder.Property(x => x.Meta).HasColumnType("DECIMAL(18,2)");
            builder.Property(x => x.PrecoUnitarioMedio).HasColumnType("DECIMAL(18,2)");
            builder.Property(x => x.ReceitaNaoCapturada).HasColumnType("DECIMAL(18,2)");
            builder.Property(x => x.CorTransparenteGrafico).HasColumnType("DECIMAL(18,2)");
            builder.Property(x => x.CorVerdeGrafico).HasColumnType("DECIMAL(18,2)");
            builder.Property(x => x.CorVermelhaGrafico).HasColumnType("DECIMAL(18,2)");
        }
    }
}