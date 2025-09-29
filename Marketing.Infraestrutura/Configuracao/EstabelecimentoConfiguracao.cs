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
            builder.HasOne(x => x.Rede).
                    WithMany(x => x.Estabelecimentos).
                    HasPrincipalKey(x => x.Nome);
            builder
                .HasMany(b => b.ExtratoVendas)
                .WithOne(p => p.Estabelecimento)
                .HasForeignKey(p => p.EstabelecimentoCnpj);

            // builder.HasMany(d => d.Contatos).WithMany(p => p.Estabelecimentos)
            //     .UsingEntity(x=>x.ToTable("EstabelecimentosContatos"));

            // builder.HasMany(x => x.MensagensMensais)
            //        .WithOne(e => e.Estabelecimento)
            //        .HasForeignKey(x => new { x.Competencia, x.ContatoTelefone, x.EstabelecimentoCnpj }); 
        }
    }
}