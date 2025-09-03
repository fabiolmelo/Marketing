using System.Linq;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Repositorio
{
    public class RepositorioContato : Repository<Contato>, IRepositorioContato
    {
        private readonly DataContext _context;
        public RepositorioContato(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }

        public async Task<List<Contato>> BuscarContatosPorEstabelecimentoComAceite(string cnpj)
        {
            var estabelecimento = await _context.Set<Estabelecimento>().FirstAsync(x => x.Cnpj == cnpj);
            return estabelecimento.Contatos.Where(x=>x.AceitaMensagem).ToList();
        }
    }
}