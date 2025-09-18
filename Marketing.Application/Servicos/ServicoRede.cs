using Marketing.Application.Servicos;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;

namespace Marketing.Domain.Interfaces.Servicos
{
    public class ServicoRede : Servico<Rede>, IServicoRede
    {
        private readonly IRepositorioRede _repositorioRede;
        public ServicoRede(IRepositorioRede repository) : base(repository)
        {
            _repositorioRede = repository;
        }

        public async Task AtualizarRedesViaPlanilha(List<DadosPlanilha> dadosPlanilhas)
        {
            foreach (DadosPlanilha linhaPlanilha in dadosPlanilhas)
            {
                var rede = await _repositorioRede.GetByIdStringAsync(linhaPlanilha.Rede);
                if (rede == null)
                {
                    rede = new Rede(linhaPlanilha.Rede);
                    await _repositorioRede.AddAsync(rede);
                }
            }
        }

        public async Task<int> BuscarRankingDoEstabelecimentoNaRede(DateTime competencia, Estabelecimento estabelecimento)
        {
            return await _repositorioRede.BuscarRankingDoEstabelecimentoNaRede(competencia, estabelecimento);
        }
    }
}