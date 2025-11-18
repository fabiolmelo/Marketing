using System.Linq.Expressions;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.PagedResponse;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioContato : Repository<Contato>, IRepositorioContato
    {
        private readonly DataContext _context;

        public RepositorioContato(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Contato>> BuscarContatosComAceite()
        {
            try
            {
                var contatos = await _context.Contatos
                                                     .Where(x=>x.AceitaMensagem == true)
                                                     .ToListAsync();
                return contatos;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<Contato?> BuscarContatosIncludeEstabelecimento(string telefone)
        {
            var contatos = await _context.Contatos.Where(x => x.Telefone == telefone)
                                        .Include(x => x.ContatoEstabelecimentos)
                                        .AsSplitQuery()
                                        //    .ThenInclude(x => x.Contato)
                                        //.AsNoTrackingWithIdentityResolution()
                                        .FirstOrDefaultAsync();
            return contatos;
        }

        public async Task<List<Contato>> BuscarContatosPorEstabelecimentoComAceite(string cnpj)
        {
            var estabelecimento = await _context.Estabelecimentos.AsNoTracking()
                                                .Include(x => x.ContatoEstabelecimentos)
                                                    .ThenInclude(x=>x.Contato)
                                                .AsSplitQuery()
                                                .FirstOrDefaultAsync(x => x.Cnpj == cnpj);
            var contatos = estabelecimento?.ContatoEstabelecimentos.Where(x => x.Contato.AceitaMensagem == true).Select(x=>x.Contato).ToList();
            return contatos ?? new List<Contato>();
        }

        public async Task<bool> EstabelecimentoPossuiContatoQueAceitaMensagem(string cnpj)
        {
            IQueryable<Estabelecimento> query = from C in _context.Contatos.Where(x => x.AceitaMensagem == true)
                                                join CE in _context.ContatoEstabelecimento on C.Telefone equals CE.ContatoTelefone
                                                join E in _context.Estabelecimentos.Where(x=>x.Cnpj == cnpj) on CE.EstabelecimentoCnpj equals E.Cnpj
                                                select E;
            var estabelecimentos = await query.ToListAsync();
            return estabelecimentos.Count > 0;
        }

        public async Task<PagedResponse<List<Contato>>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<Contato, bool>>? filtros = null, params Expression<Func<Contato, object>>[] includes)
        {
            var query = _context.Contatos.AsNoTracking();
            
            if (filtros != null)
            {
                query.Where(filtros);
            }
            if (includes != null)
            {
                includes.Aggregate(query, (current, includes) => current.Include(includes));
            }
            query = query.OrderBy(x => x.Telefone).AsQueryable();
            
            var totalRecords = await query.CountAsync();
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return new PagedResponse<List<Contato>>(await query.ToListAsync(), pageNumber, pageSize, totalRecords);
        }
    }
}