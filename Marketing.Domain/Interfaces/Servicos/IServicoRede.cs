using Marketing.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoRede : IServico<Rede>
    {
        Task AtualizarRedesViaPlanilha(List<DadosPlanilha> dadosPlanilhas);
        Task<int> BuscarRankingDoEstabelecimentoNaRede(DateTime competencia, Estabelecimento estabelecimento);
    }
}