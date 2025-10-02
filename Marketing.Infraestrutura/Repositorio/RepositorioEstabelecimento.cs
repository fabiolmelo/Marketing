using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.PagedResponse;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioEstabelecimento : Repository<Estabelecimento>, IRepositorioEstabelecimento
    {
        private readonly DataContext _context;
        public RepositorioEstabelecimento(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }

        public async Task<Estabelecimento?> FindEstabelecimentoIncludeContatoRede(string cnpj)
        {
            var estabelecimentos = await _context.Set<Estabelecimento>()
                                                .Include(x => x.ContatoEstabelecimentos)
                                                    .ThenInclude(X=>X.Contato)
                                                .Include(x => x.Rede)
                                                .Include(x => x.ExtratoVendas)
                                                .ToListAsync();
            var estabelecimento = estabelecimentos.Find(x => x.Cnpj == cnpj);
            return estabelecimento;
        }

        public async Task<Estabelecimento?> FindEstabelecimentoPorCnpj(string cnpj)
        {
            var estabelecimentos = await _context.Set<Estabelecimento>()
                                 .Include(x => x.Rede)
                                 .Include(x => x.ContatoEstabelecimentos)
                                    .ThenInclude(X=>X.Estabelecimento)
                                 .ToListAsync();
            var estabelecimento = estabelecimentos.Find(x => x.Cnpj == cnpj);
            return estabelecimento;
        }

        public async Task<PagedResponse<List<Estabelecimento>>> GetAllEstabelecimentos(int pageNumber, int pageSize, string? filtro)
        {
            var query = _context.Set<Estabelecimento>().AsQueryable();
            if (filtro != null)
            {
                query.Where(x=>x.RazaoSocial == filtro || x.Cidade == filtro);
            }
            
            var totalRecords = await query.CountAsync();
            query = query.Skip(pageNumber - 1)
                         .Take(pageSize)
                         .Include(x => x.Rede)
                         .Include(x => x.ContatoEstabelecimentos)
                         .ThenInclude(X=>X.Contato); 
            return new PagedResponse<List<Estabelecimento>>(await query.ToListAsync(), pageNumber, pageSize, totalRecords);
        }
    }
}
