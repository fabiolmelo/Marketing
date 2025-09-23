using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioExtratoVendas : Repository<ExtratoVendas>, IRepositorioExtratoVendas
    {
        private readonly DataContext _context;
        public RepositorioExtratoVendas(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }

        public async Task<DateTime> BuscarCompetenciaVigente()
        {
            return await _context.Set<ExtratoVendas>().AsNoTracking().MaxAsync(x => x.Competencia);
        }
    }
}