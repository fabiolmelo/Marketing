using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class ExtratoVendaConfiguracao : IEntityTypeConfiguration<ExtratoVendas>
    {
        public void Configure(EntityTypeBuilder<ExtratoVendas> builder)
        {
            builder.HasKey(x => new {x.Ano, x.Mes, x.EstabelecimentoCnpj});
            builder.Property(x=>x.IncidenciaReal).HasPrecision(2);
            builder.Property(x=>x.Meta).HasPrecision(2);
            builder.Property(x=>x.PrecoUnitarioMedio).HasPrecision(2);
            builder.Property(x=>x.ReceitaNaoCapturada).HasPrecision(2);
        }
    }
}