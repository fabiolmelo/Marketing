using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.IdentityModel.Tokens;
using Marketing.Domain.PagedResponse;
using System.Linq.Expressions;
using Marketing.Domain.Interfaces.IUnityOfWork;

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
                var contato = await _unitOfWork.repositorioContato.BuscarContatosIncludeEstabelecimento(item.Fone);
                var estabelecimento = await _unitOfWork.repositorioEstabelecimento.GetByIdStringAsync(item.Cnpj);

                if (contato != null && estabelecimento != null)
                {
                    if (!contato.ContatoEstabelecimentos.Any(x => x.Estabelecimento.Cnpj == estabelecimento.Cnpj))
                    {
                        //contato.Estabelecimentos.Add(estabelecimento);
                        //_unitOfWork.repositorioContato.Update(contato);
                        await _unitOfWork.CommitAsync();
                    }
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

        public async Task<PagedResponse<Contato>> GetAllContatos(int pageNumber, int pageSize, Expression<Func<Contato, bool>>? filtros = null)
        {
            return await _unitOfWork.repositorioContato.GetAllAsync(pageNumber, pageSize, filtros);
        }
       
    }
}
