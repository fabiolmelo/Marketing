using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioExtratoVendas : IRepository<ExtratoVendas>
    {
        Task<DateTime> BuscarCompetenciaVigente();
    }
}