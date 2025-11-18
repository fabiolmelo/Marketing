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

        public async Task<DateTime?> BuscarCompetenciaVigente()
        {
            if (_context.Set<ExtratoVendas>().Any() == false) return null;
            var competencias = await _context.Set<ExtratoVendas>()
                                             .Select(x=>x.Competencia)
                                             .Distinct()
                                             .ToListAsync();
            return competencias.Max();
        }
    }
}