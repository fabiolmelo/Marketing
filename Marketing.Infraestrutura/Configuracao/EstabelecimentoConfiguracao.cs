using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class EstabelecimentoConfiguracao : IEntityTypeConfiguration<Estabelecimento>
    {
        public void Configure(EntityTypeBuilder<Estabelecimento> builder)
        {
            builder.HasKey(x => x.Cnpj);
            builder.Property(x => x.Cnpj).HasMaxLength(14);
            builder.Property(x => x.Uf).HasMaxLength(2);
            builder.Property(x => x.Cidade).HasMaxLength(100);

            builder.HasMany(r => r.ExtratoVendas)
                   .WithOne(e => e.Estabelecimento)
                   .HasForeignKey(e => e.EstabelecimentoCnpj);

            builder.HasOne(x => x.Rede).
                    WithMany(x => x.Estabelecimentos).
                    HasPrincipalKey(x => x.Nome);

            // builder.HasMany(x => x.MensagensMensais)
            //        .WithOne(e => e.Estabelecimento)
            //        .HasForeignKey(x => new { x.Competencia, x.ContatoTelefone, x.EstabelecimentoCnpj }); 
        }
    }
}