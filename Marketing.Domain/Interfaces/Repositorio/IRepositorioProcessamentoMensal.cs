using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioProcessamentoMensal : IRepository<FechamentoMensal>
    {
        Task<List<Estabelecimento>> GetAllEstabelecimentosParaGerarPdf(DateTime competencia); 
    }
}
