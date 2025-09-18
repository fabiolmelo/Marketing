using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.IdentityModel.Tokens;
using Marketing.Domain.PagedResponse;
using System.Linq.Expressions;
using Marketing.Domain.Interfaces.Repositorio;

namespace Marketing.Application.Servicos
{
    public class ServicoContato : Servico<Contato>, IServicoContato
    {
        private readonly IRepository<Contato> _repositorioContato;
        private readonly IRepositorioContato _irepositorioContato;

        public ServicoContato(IRepository<Contato> repository, IRepositorioContato irepositorioContato) : base(repository)
        {
            _repositorioContato = repository;
            _irepositorioContato = irepositorioContato;
        }

        public async Task AtualizarContatosViaPlanilha(List<DadosPlanilha> dadosPlanilhas)
        {
            foreach (DadosPlanilha linhaPlanilha in dadosPlanilhas)
            {
                if (!linhaPlanilha.Fone.IsNullOrEmpty())
                {
                    var contato = await _repositorioContato.GetByIdStringAsync(linhaPlanilha.Fone);
                    if (contato == null)
                    {
                        contato = new Contato(linhaPlanilha.Fone);
                        await _repositorioContato.AddAsync(contato);
                        
                    }
                }
            }
        }

        public async Task<List<Contato>> BuscarContatosPorEstabelecimentoComAceite(string cnpj)
        {
            return await _irepositorioContato.BuscarContatosPorEstabelecimentoComAceite(cnpj);
        }

        public async Task<PagedResponse<Contato>> GetAllContatos(int pageNumber, int pageSize, Expression<Func<Contato, bool>>? filtros = null)
        {
            return await _repositorioContato.GetAllAsync(pageNumber, pageSize, filtros);
        }
       
    }
}
