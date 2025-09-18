using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class ImportacaoEfetuadaConfiguracao : IEntityTypeConfiguration<ImportacaoEfetuada>
    {
        public void Configure(EntityTypeBuilder<ImportacaoEfetuada> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.NomeArquivoServer).HasColumnType("VARCHAR(255)");
            builder.Property(x => x.DataImportacao).HasColumnType("DATETIME");

            builder.HasMany(x=>x.DadosPlanilha).
                    WithOne(x=>x.ImportacaoEfetuada).
                    HasForeignKey(x=>x.ImportacaoEfetuadaId);
        }
    }
}