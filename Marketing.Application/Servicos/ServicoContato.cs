using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.UnitOfWork;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.IdentityModel.Tokens;

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
            var telefones = dadosPlanilhas.Select(x => x.Fone).Distinct();
            foreach (string fone in telefones)
            {
                if (!fone.IsNullOrEmpty())
                {
                    var foneCadastrado = await _unitOfWork.GetRepository<Contato>().
                                            GetByIdStringAsync(fone);
                    if (foneCadastrado == null)
                    {
                        await _unitOfWork.GetRepository<Contato>().
                                            AddAsync(new Contato(fone));
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
        }
    }
}
