using Marketing.Domain.Entidades;
using Marketing.Domain.Extensoes;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;
using Marketing.Domain.Interfaces.UnitOfWork;

namespace Marketing.Application.Servicos
{
    public class ServicoProcessamentoMensal : IServicoProcessamentoMensal
    {
        private readonly IRepositorioProcessamentoMensal _repositorioProcessamentoMensal;
        private readonly IServicoArquivos _servicoArquivos;
        private readonly IServicoRede _servicoRede;
        private readonly IServicoGrafico _servicoGrafico;
        private readonly IUnitOfWork _unitOfWork;

        public ServicoProcessamentoMensal(IRepositorioProcessamentoMensal repositorioProcessamentoMensal,
                                          IServicoArquivos servicoArquivos,
                                          IServicoRede servicoRede,
                                          IServicoGrafico servicoGrafico,
                                          IUnitOfWork unitOfWork)
        {
            _repositorioProcessamentoMensal = repositorioProcessamentoMensal;
            _servicoArquivos = servicoArquivos;
            _servicoRede = servicoRede;
            _servicoGrafico = servicoGrafico;
            _unitOfWork = unitOfWork;
        }

        public async Task GerarProcessamentoMensal(DateTime competencia,
                                                   String contentRootPath,
                                                   string caminhoApp)
        {
            var estabelecimentos = await _repositorioProcessamentoMensal.GetAllEstabelecimentosParaGerarPdf(competencia);
            string mes = competencia.ToString("MMMM yyyy").ToLower().PriMaiuscula();

            //var fechamentoMensal = new FechamentoMensal();

            try
            {
                foreach (Estabelecimento estabelecimento in estabelecimentos)
                {
                    if (estabelecimento.ExtratoVendas.Count > 0)
                    {
                        var posicaoNaRede = await _servicoRede.BuscarRankingDoEstabelecimentoNaRede(competencia, estabelecimento);
                        var arquivoPdf = $"{estabelecimento.Cnpj}-{estabelecimento.RazaoSocial}.pdf";
                        _servicoGrafico.GerarGrafico(estabelecimento, contentRootPath);
                        _servicoArquivos.GerarArquivoPdf(estabelecimento, arquivoPdf,
                                                         posicaoNaRede, contentRootPath, caminhoApp);
                        var estabelecimentoUpdate = await _unitOfWork.GetRepository<Estabelecimento>().
                                                                      GetByIdStringAsync(estabelecimento.Cnpj);
                        if (estabelecimentoUpdate != null) {
                            estabelecimentoUpdate.UltimoPdfGerado = $"{Path.Combine(contentRootPath, arquivoPdf)}";
                            _unitOfWork.GetRepository<Estabelecimento>().Update(estabelecimentoUpdate);
                            await _unitOfWork.CommitAsync();    
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Erro {ex.Message}");
            }
        }
    }
}
