using System.Text.Json;
using Marketing.Domain.Entidades;
using Marketing.Domain.Extensoes;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;

namespace Marketing.Application.Servicos
{
    public class ServicoProcessamentoMensal : IServicoProcessamentoMensal
    {
        private readonly IServicoMeta _servicoMeta;
        private readonly IServicoRede _servicoRede;
        private readonly IServicoMensagemEnviada _servicoMensagemEnviada;
        private readonly IServicoEstabelecimento _servicoEstabelecimento;
        private readonly IRepositorioProcessamentoMensal _repositorioProcessamentoMensal;
        private readonly IRepositorioContato _repositorioContato;


        public ServicoProcessamentoMensal(IRepositorioProcessamentoMensal repositorioProcessamentoMensal,
                                          IServicoMeta servicoMeta,
                                          IRepositorioContato repositorioContato,
                                          IServicoGrafico servicoGrafico,
                                          IServicoArquivos servicoArquivos,
                                          IServicoMensagemEnviada servicoMensagemEnviada,
                                          IServicoRede servicoRede,
                                          IServicoEstabelecimento servicoEstabelecimento)
        {
            _repositorioProcessamentoMensal = repositorioProcessamentoMensal;
            _servicoMeta = servicoMeta;
            _repositorioContato = repositorioContato;
            ServicoGrafico = servicoGrafico;
            ServicoArquivos = servicoArquivos;
            _servicoMensagemEnviada = servicoMensagemEnviada;
            _servicoRede = servicoRede;
            _servicoEstabelecimento = servicoEstabelecimento;
        }

        public IServicoGrafico ServicoGrafico { get; }

        public IServicoArquivos ServicoArquivos { get; }

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
                    var contatos = await _repositorioContato.BuscarContatosPorEstabelecimentoComAceite(estabelecimento.Cnpj);
                    foreach (Contato contato in contatos)
                    {
                        var mensagem = new EnvioMensagemMensal(competencia,
                                                               estabelecimento.Cnpj,
                                                               estabelecimento,
                                                               contato.Telefone ?? "",
                                                               contato);
                        
                        // ServicoExtratoResponseDto response = await _servicoMeta.EnviarExtrato(contato, estabelecimento, caminhoApp);
                        // if (response.IsSuccessStatusCode)
                        // {
                        //     WhatsAppResponseResult? json = JsonSerializer.Deserialize<WhatsAppResponseResult>(response.Response, JsonSerializerOptions.Default);
                        //     if (json != null && contato.Telefone != null)
                        //     {
                        //         foreach (Message message in json.messages)
                        //         {
                        //             var mensagemId = message.id;
                        //             if (mensagemId != null)
                        //             {
                        //                 // var length = mensagemId.Length;
                        //                 // var mensagem = new MensagemEnviada(mensagemId);
                        //                 // //mensagem.AdicionarEvento(MensagemStatus.sent);
                        //                 // await _servicoMensagemEnviada.AddAsyncWithCommit(mensagem);
                        //             }
                        //         }
                        //     }
                        // }
                    }
                }
            }
            catch (Exception ex)
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
                ServicoGrafico.GerarGrafico(estabelecimento, contentRootPath);
                ServicoArquivos.GerarArquivoPdf(estabelecimento, arquivoPdf,
                                                    posicaoNaRede, contentRootPath, caminhoApp);
                var estabelecimentoUpdate = await _servicoEstabelecimento.GetByIdStringAsync(estabelecimento.Cnpj);
                if (estabelecimentoUpdate != null)
                {
                    estabelecimentoUpdate.UltimoPdfGerado = $"{arquivoPdf}";
                    _servicoEstabelecimento.Update(estabelecimentoUpdate);
                }
            }                                    
        }
    }
}
