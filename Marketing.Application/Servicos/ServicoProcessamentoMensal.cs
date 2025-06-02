using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Marketing.Domain.Entidades;
using Marketing.Domain.Extensoes;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;

namespace Marketing.Application.Servicos
{
    public class ServicoProcessamentoMensal : IServicoProcessamentoMensal
    {
        private readonly IRepositorioProcessamentoMensal _repositorioProcessamentoMensal;
        private readonly IServicoArquivos _servicoArquivos;
        private readonly IServicoRede _servicoRede;

        public ServicoProcessamentoMensal(IRepositorioProcessamentoMensal repositorioProcessamentoMensal,
                                          IServicoArquivos servicoArquivos,
                                          IServicoRede servicoRede)
        {
            _repositorioProcessamentoMensal = repositorioProcessamentoMensal;
            _servicoArquivos = servicoArquivos;
            _servicoRede = servicoRede;
        }

        public async Task GerarProcessamentoMensal(DateTime competencia, String contentRootPath)
        {
            var estabelecimentos = await _repositorioProcessamentoMensal.GetAllEstabelecimentosParaGerarPdf(competencia);
            string mes = competencia.ToString("MMM-yyyy").ToLower().PriMaiuscula();

            //var fechamentoMensal = new FechamentoMensal();

            
            foreach(Estabelecimento estabelecimento in estabelecimentos){
                var posicaoNaRede = 1;  //await _servicoRede.BuscarRankingDoEstabelecimentoNaRede(competencia, estabelecimento);
                var arquivoPdf = $"{estabelecimento.RazaoSocial}-{mes}.pdf";
                _servicoArquivos.GerarArquivoPdf(estabelecimento, arquivoPdf, posicaoNaRede, contentRootPath);
            }
        }
    }
}
