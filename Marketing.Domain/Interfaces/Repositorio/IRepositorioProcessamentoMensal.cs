using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioProcessamentoMensal : IRepository<EnvioMensagemMensal>
    {
        Task<List<Estabelecimento>> GetAllEstabelecimentosParaGerarPdf(DateTime competencia); 
    }
}
