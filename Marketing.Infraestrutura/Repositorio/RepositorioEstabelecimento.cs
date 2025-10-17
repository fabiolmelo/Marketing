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
                                 .Include(x => x.ExtratoVendas)
                                 .ToListAsync();
            var estabelecimento = estabelecimentos.Find(x => x.Cnpj == cnpj);
            return estabelecimento;
        }

        public async Task<List<Estabelecimento>> GetAllEstabelecimentoPorContato(string telefone)
        {
            IQueryable<Estabelecimento> query = from C in _context.Contatos.Where(x => x.Telefone == telefone)
                                                join CE in _context.ContatoEstabelecimento on C.Telefone equals CE.ContatoTelefone
                                                join E in _context.Estabelecimentos on CE.EstabelecimentoCnpj equals E.Cnpj
                                                select E;
            return await query.ToListAsync();
        }

        public async Task<PagedResponse<List<Estabelecimento>>> GetAllEstabelecimentos(int pageNumber, int pageSize, string? filtro)
        {
            var query = _context.Set<Estabelecimento>().AsQueryable();
            if (filtro != null)
            {
                query.Where(x => x.RazaoSocial == filtro || x.Cidade == filtro);
            }
            query = query.OrderBy(x => x.Cnpj);
            
            var totalRecords = await query.CountAsync();
            query = query.Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .Include(x => x.Rede)
                         .Include(x => x.ContatoEstabelecimentos)
                         .ThenInclude(X=>X.Contato); 
            return new PagedResponse<List<Estabelecimento>>(await query.ToListAsync(), pageNumber, pageSize, totalRecords);
        }
    }
}
