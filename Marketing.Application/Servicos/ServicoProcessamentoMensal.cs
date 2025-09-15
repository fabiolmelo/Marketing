using System.Text.Json;
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
        private readonly IRepositorioContato _repositorioContato;
        private readonly IServicoArquivos _servicoArquivos;
        private readonly IServicoRede _servicoRede;
        private readonly IServicoGrafico _servicoGrafico;
        private readonly IServicoMeta _servicoMeta;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServicoMensagem _servicoMensagem;

        public ServicoProcessamentoMensal(IRepositorioProcessamentoMensal repositorioProcessamentoMensal,
                                          IServicoArquivos servicoArquivos,
                                          IServicoRede servicoRede,
                                          IServicoGrafico servicoGrafico,
                                          IUnitOfWork unitOfWork,
                                          IRepositorioContato repositorioContato,
                                          IServicoMeta servicoMeta,
                                          IServicoMensagem servicoMensagem)
        {
            _repositorioProcessamentoMensal = repositorioProcessamentoMensal;
            _servicoArquivos = servicoArquivos;
            _servicoRede = servicoRede;
            _servicoGrafico = servicoGrafico;
            _unitOfWork = unitOfWork;
            _repositorioContato = repositorioContato;
            _servicoMeta = servicoMeta;
            _servicoMensagem = servicoMensagem;
        }

        public async Task GerarProcessamentoMensal(DateTime competencia,
                                                   String contentRootPath,
                                                   string caminhoApp)
        {
            var estabelecimentos = await _repositorioProcessamentoMensal.GetAllEstabelecimentosParaGerarPdf(competencia);
            string mes = competencia.ToString("MMMM yyyy").ToLower().PriMaiuscula();

            try
            {
                foreach (Estabelecimento estabelecimento in estabelecimentos)
                {
                    await GerarProcessamentoPorEstabelecimento(estabelecimento, competencia,
                                                               contentRootPath, caminhoApp);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Erro {ex.Message}");
            }
        }

        public async Task GerarProcessamentoPorEstabelecimento(Estabelecimento estabelecimento,
                        DateTime competencia, string contentRootPath, string caminhoApp)
        {
            if (estabelecimento.ExtratoVendas.Count > 0)
            {
                var posicaoNaRede = await _servicoRede.BuscarRankingDoEstabelecimentoNaRede(competencia, estabelecimento);
                var arquivoPdf = $"{estabelecimento.Cnpj}-{estabelecimento.RazaoSocial.Replace(" ","_")}.pdf";
                _servicoGrafico.GerarGrafico(estabelecimento, contentRootPath);
                _servicoArquivos.GerarArquivoPdf(estabelecimento, arquivoPdf,
                                                    posicaoNaRede, contentRootPath, caminhoApp);
                var estabelecimentoUpdate = await _unitOfWork.GetRepository<Estabelecimento>().
                                                                GetByIdStringAsync(estabelecimento.Cnpj);
                if (estabelecimentoUpdate != null)
                {
                    estabelecimentoUpdate.UltimoPdfGerado = $"{arquivoPdf}";
                    _unitOfWork.GetRepository<Estabelecimento>().Update(estabelecimentoUpdate);

                    var contatos = await _repositorioContato.BuscarContatosPorEstabelecimentoComAceite(estabelecimentoUpdate.Cnpj);
                    foreach (Contato contato in contatos)
                    {
                        var response = await _servicoMeta.EnviarExtrato(contato, estabelecimentoUpdate, caminhoApp);
                        if (response.IsSuccessStatusCode)
                        {
                            var json = JsonSerializer.Deserialize<WhatsAppResponseResult>(response.Response);
                            if (json != null && contato.Telefone != null)
                            {
                                foreach (Message message in json.messages)
                                {
                                    var mensagemId = message.id;
                                    if (mensagemId != null)
                                    {
                                        var mensagem = new Mensagem(mensagemId, contato.Telefone, contato);
                                        //mensagem.AdicionarEvento(MensagemStatus.sent);
                                        await _servicoMensagem.AddAsync(mensagem);
                                    }
                                }
                            }
                        }
                    }
                }
            }                                    
        }
    }
}
