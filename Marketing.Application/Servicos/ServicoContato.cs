using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.IdentityModel.Tokens;
using Marketing.Domain.PagedResponse;
using System.Linq.Expressions;
using Marketing.Domain.Interfaces.IUnitOfWork;

namespace Marketing.Application.Servicos
{
    public class ServicoContato : Servico<Contato>, IServicoContato
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicoContato(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AtualizarAssociacaoContatoEstabelecimento(List<DadosPlanilha> dadosPlanilhas)
        {
            var group = dadosPlanilhas.Where(x => !x.Fone.IsNullOrEmpty())
                                      .GroupBy(item => new { item.Cnpj, item.Fone})
                                      .Select(g => new
                                      {
                                        Cnpj = g.Key.Cnpj,
                                        Fone = g.Key.Fone
                                      });

                    
            foreach (var item in group)
            {
                var contatoEstabelecimento = await _unitOfWork.GetRepository<ContatoEstabelecimento>()
                                                              .GetByIdChaveComposta(item.Fone, item.Cnpj);
                if (contatoEstabelecimento == null)
                {
                    contatoEstabelecimento = new ContatoEstabelecimento(item.Fone, item.Cnpj);
                    await _unitOfWork.GetRepository<ContatoEstabelecimento>().AddAsync(contatoEstabelecimento);
                    await _unitOfWork.CommitAsync();
                }
            }
        }

        public async Task AtualizarContatosViaPlanilha(List<DadosPlanilha> dadosPlanilhas)
        {
            foreach (DadosPlanilha linhaPlanilha in dadosPlanilhas)
            {
                if (!linhaPlanilha.Fone.IsNullOrEmpty())
                {
                    var contato = await _unitOfWork.repositorioContato.GetByIdStringAsync(linhaPlanilha.Fone);
                    if (contato == null)
                    {
                        contato = new Contato(linhaPlanilha.Fone);
                        await _unitOfWork.repositorioContato.AddAsync(contato);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
        }

        public async Task<List<Contato>> BuscarContatosPorEstabelecimentoComAceite(string cnpj)
        {
            return await _unitOfWork.repositorioContato.BuscarContatosPorEstabelecimentoComAceite(cnpj);
        }

        public async Task<PagedResponse<List<Contato>>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<Contato, bool>>? filtros = null, params Expression<Func<Contato, object>>[] includes)
        {
            return await _unitOfWork.repositorioContato.GetAllAsync(pageNumber, pageSize, filtros, includes);
        }

        public async Task<PagedResponse<List<Contato>>> GetAllContatos(int pageNumber, int pageSize, Expression<Func<Contato, bool>>? filtros = null)
        {
            return await _unitOfWork.repositorioContato.GetAllAsync(pageNumber, pageSize, filtros);
        }
    }
}
