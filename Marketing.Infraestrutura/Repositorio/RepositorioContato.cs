using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
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

        public async Task<Contato> BuscarContatosIncludeEstabelecimento(string telefone)
        {
            var contato = await _context.Set<Contato>()
                                        .Include(x => x.Estabelecimentos)
                                        .Where(x => x.Telefone == telefone)
                                        .FirstAsync();
            return contato;
        }

        public async Task<List<Contato>> BuscarContatosPorEstabelecimentoComAceite(string cnpj)
        {
            var estabelecimento = await _context.Set<Estabelecimento>().
                                                 Include(x=>x.Contatos).
                                                 Where(x => x.Cnpj == cnpj).
                                                 FirstAsync();
            var contatos = estabelecimento.Contatos.Where(x => x.AceitaMensagem).ToList();
            return contatos ?? new List<Contato>();
        }
    }
}