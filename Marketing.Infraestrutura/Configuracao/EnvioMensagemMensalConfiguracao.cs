using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class EnvioMensagemMensalConfiguracao : IEntityTypeConfiguration<EnvioMensagemMensal>
    {
        public void Configure(EntityTypeBuilder<EnvioMensagemMensal> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.Competencia, x.ContatoTelefone, x.EstabelecimentoCnpj, x.DataGeracao },"IX_MENSAGEM");
            
            builder
                .HasOne(x => x.Mensagem)
                .WithOne(x => x.EnvioMensagemMensal)
                .HasForeignKey<EnvioMensagemMensal>(bh => bh.MensagemId); 
        }
    }
}