using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoExtratoVendas : IServico<ExtratoVendas>
    {
        Task AtualizarExtratosViaPlanilha(List<DadosPlanilha> dadosPlanilhas);
        Task<DateTime> BuscarCompetenciaVigente();
    }
}