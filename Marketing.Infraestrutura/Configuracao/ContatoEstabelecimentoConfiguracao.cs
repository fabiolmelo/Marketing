using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class ContatoEstabelecimentoConfiguracao : IEntityTypeConfiguration<ContatoEstabelecimento>
    {
        public void Configure(EntityTypeBuilder<ContatoEstabelecimento> builder)
        {
            builder.HasKey(x => new { x.ContatoTelefone, x.EstabelecimentoCnpj });

            builder.HasOne(x => x.Contato)
                   .WithMany(x => x.ContatoEstabelecimentos)
                   .HasForeignKey(x => x.ContatoTelefone); 
            builder.HasOne(x => x.Estabelecimento)
                   .WithMany(x => x.ContatoEstabelecimentos)
                   .HasForeignKey(x => x.EstabelecimentoCnpj);
        }
    }
}