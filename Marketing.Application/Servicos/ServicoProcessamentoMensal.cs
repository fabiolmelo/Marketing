using Marketing.Domain.Entidades;
using Marketing.Domain.Extensoes;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.Extensions.Logging;

namespace Marketing.Application.Servicos
{
    public class ServicoProcessamentoMensal : IServicoProcessamentoMensal
    {
        private readonly IServicoGraficoRevisado _servicoGraficoRevisado;
        private readonly ILogger<ServicoProcessamentoMensal> _logger;
        private readonly IRepositorioEstabelecimento _repositorioEstabelecimento;
        private readonly IRepositorioEnvioMensagemMensal _repositorioEnvioMensagemMensal;
        private readonly IRepositorioRede _repositorioRede;

        public ServicoProcessamentoMensal(IServicoGraficoRevisado servicoGraficoRevisado,
                                          ILogger<ServicoProcessamentoMensal> logger,
                                          IRepositorioEstabelecimento repositorioEstabelecimento,
                                          IRepositorioEnvioMensagemMensal repositorioEnvioMensagemMensal,
                                          IRepositorioRede repositorioRede)
        {
            _servicoGraficoRevisado = servicoGraficoRevisado;
            _logger = logger;
            _repositorioEstabelecimento = repositorioEstabelecimento;
            _repositorioEnvioMensagemMensal = repositorioEnvioMensagemMensal;
            _repositorioRede = repositorioRede;
        }

        public async Task GerarProcessamentoMensal(DateTime competencia,
                                                   String contentRootPath,
                                                   string caminhoApp, 
                                                   List<Contato> contatos)
        {   
            string mes = competencia.ToString("MMMM yyyy").ToLower().PriMaiuscula();
            foreach (Contato contato in contatos)
            {
                if (contato.Telefone == null || String.IsNullOrEmpty(contato.Telefone)) throw new Exception("TELEFONE NULO NO CADASTRO");
                var estabelecimentos = await _repositorioEstabelecimento.GetAllEstabelecimentoPorContatoQuePossuiCompetenciaVigente(contato.Telefone);
                foreach(Estabelecimento estabelecimento in estabelecimentos)
                {
                    var estabelecimentoPdf = await _repositorioEstabelecimento.FindEstabelecimentoPorCnpjParaPdf(estabelecimento.Cnpj, competencia);
                    if (estabelecimentoPdf == null) throw new Exception("ESTABELECIMENTO COM DADOS CORROMPIDOS");
                    await GerarProcessamentoPorEstabelecimento(estabelecimentoPdf, competencia,
                                                            contentRootPath, caminhoApp);
                    var telefone = contato.Telefone ?? "";
                    var mensagem = new EnvioMensagemMensal(competencia, estabelecimentoPdf.Cnpj, telefone,
                                                            estabelecimentoPdf?.RedeNome ?? "",
                                                            estabelecimentoPdf?.RazaoSocial ?? "");
                    _repositorioEnvioMensagemMensal.Add(mensagem);
                    _repositorioEnvioMensagemMensal.Commit();
                }
            }
        }

        public async Task GerarProcessamentoPorEstabelecimento(Estabelecimento estabelecimento,
                        DateTime competencia, string contentRootPath, string caminhoApp)
        {
            if (estabelecimento.ExtratoVendas.Count > 0)
            {
                var posicaoNaRede = await _repositorioRede.BuscarRankingDoEstabelecimentoNaRede(competencia, estabelecimento);
                var arquivoPdf = $"{estabelecimento.Cnpj}-{estabelecimento.RazaoSocial?.Replace(" ","_")}.pdf";
                _servicoGraficoRevisado.GerarGrafico(estabelecimento, contentRootPath);
                _servicoGraficoRevisado.GerarArquivoPdf(estabelecimento, arquivoPdf,
                                                    posicaoNaRede, contentRootPath, caminhoApp);
                var estabelecimentoUpdate = _repositorioEstabelecimento.GetByIdString(estabelecimento.Cnpj);
                if (estabelecimentoUpdate != null)
                {
                    estabelecimentoUpdate.UltimoPdfGerado = $"{arquivoPdf}";
                    _repositorioEstabelecimento.Update(estabelecimentoUpdate);
                    _repositorioEstabelecimento.Commit();
                }
            }     
        }
    }
}
