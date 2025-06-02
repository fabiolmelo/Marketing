using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class EstabelecimentoContatoConfiguracao : IEntityTypeConfiguration<EstabelecimentoContato>
    {
        public void Configure(EntityTypeBuilder<EstabelecimentoContato> builder)
        {
            builder.HasKey(x => new { x.EstabelecimentoCnpj, x.ContatoTelefone });
        }
    }
}