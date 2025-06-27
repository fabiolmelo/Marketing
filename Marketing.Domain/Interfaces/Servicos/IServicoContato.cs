using Marketing.Domain.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoContato : IServico<Contato>
    {
        Task AtualizarContatosViaPlanilha(List<DadosPlanilha> dadosPlanilhas);
    }
}
