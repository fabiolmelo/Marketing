using Marketing.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Infraestrutura.Configuracao
{
    public class EnvioMensagemMensalConfiguracao : IEntityTypeConfiguration<EnvioMensagemMensal>
    {
        public void Configure(EntityTypeBuilder<EnvioMensagemMensal> builder)
        {
            builder.HasKey(x => new { x.Competencia, x.ContatoTelefone, x.EstabelecimentoCnpj });
            // builder.HasOne(x => x.Contato).WithMany(x => x.MensagensMensais).HasForeignKey(x => x.ContatoTelefone);
            // builder.HasOne(x => x.Estabelecimento).WithMany(x => x.MensagensMensais).HasForeignKey(x => x.EstabelecimentoCnpj);
            builder.HasOne(x => x.Mensagem).WithMany(x => x.MensagensMensais).HasForeignKey(x => x.MensagemId).IsRequired(false);
        }
    }
}