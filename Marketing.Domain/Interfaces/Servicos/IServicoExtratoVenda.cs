using Marketing.Domain.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoExtratoVendas : IServico<ExtratoVendas>
    {
        Task AtualizarExtratosViaPlanilha(List<DadosPlanilha> dadosPlanilhas);
    }
}