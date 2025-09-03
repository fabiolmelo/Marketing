using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.UnitOfWork;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.IdentityModel.Tokens;
using Marketing.Domain.PagedResponse;
using System.Linq.Expressions;

namespace Marketing.Application.Servicos
{
    public class ServicoContato : Servico<Contato>, IServicoContato
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicoContato(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AtualizarContatosViaPlanilha(List<DadosPlanilha> dadosPlanilhas)
        {
            foreach (DadosPlanilha linhaPlanilha in dadosPlanilhas)
            {
                if (!linhaPlanilha.Fone.IsNullOrEmpty())
                {
                    var contato = await _unitOfWork.GetRepository<Contato>().
                                            GetByIdStringAsync(linhaPlanilha.Fone);
                    if (contato == null)
                    {
                        contato = new Contato(linhaPlanilha.Fone);
                        await _unitOfWork.GetRepository<Contato>().AddAsync(contato);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
        }

        public async Task<PagedResponse<Contato>> GetAllContatos(int pageNumber, int pageSize, Expression<Func<Contato, bool>>? filtros = null)
        {
            return await _unitOfWork.GetRepository<Contato>().GetAllAsync(pageNumber, pageSize, filtros);
        }
       
    }
}
