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
                                                .Include(x => x.Contatos)
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
                                 .Include(x => x.Contatos)
                                 .ToListAsync();
            var estabelecimento = estabelecimentos.Find(x => x.Cnpj == cnpj);
            return estabelecimento;
        }

        public async Task<PagedResponse<Estabelecimento>> GetAllEstabelecimentos(int pageNumber, int pageSize, string? filtro)
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
                         .Include(x=>x.Contatos); 
            return new PagedResponse<Estabelecimento>(await query.ToListAsync(), pageNumber, pageSize, totalRecords);
        }
    }
}
