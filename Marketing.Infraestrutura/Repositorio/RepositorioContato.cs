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

        public async Task<Contato?> BuscarContatosIncludeEstabelecimento(string telefone)
        {
            var contatos = await _context.Contatos.Where(x => x.Telefone == telefone)
                                        .Include(x => x.ContatoEstabelecimentos)
                                            .ThenInclude(x=>x.Contato)
                                        .FirstOrDefaultAsync();
            return contatos;
        }

        public async Task<List<Contato>> BuscarContatosPorEstabelecimentoComAceite(string cnpj)
        {
            var estabelecimento = await _context.Estabelecimentos
                                                .Include(x => x.ContatoEstabelecimentos)
                                                    .ThenInclude(x=>x.Estabelecimento)
                                                .FirstOrDefaultAsync(x => x.Cnpj == cnpj);
            var contatos = estabelecimento?.ContatoEstabelecimentos.Where(x => x.Contato.AceitaMensagem).Select(x=>x.Contato).ToList();
            return contatos ?? new List<Contato>();
        }
    }
}