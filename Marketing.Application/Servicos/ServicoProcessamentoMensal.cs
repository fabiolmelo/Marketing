using Marketing.Domain.Entidades;
using Marketing.Domain.Extensoes;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Servicos;

namespace Marketing.Application.Servicos
{
    public class ServicoProcessamentoMensal : IServicoProcessamentoMensal
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServicoGraficoRevisado _servicoGrafico;

        public ServicoProcessamentoMensal(IUnitOfWork unitOfWork, IServicoGraficoRevisado servicoGrafico)
        {
            _unitOfWork = unitOfWork;
            _servicoGrafico = servicoGrafico;
        }

        public async Task GerarProcessamentoMensal(DateTime competencia,
                                                   String contentRootPath,
                                                   string caminhoApp)
        {
            var contatos = await _unitOfWork.repositorioContato.BuscarContatosComAceite();
            string mes = competencia.ToString("MMMM yyyy").ToLower().PriMaiuscula();

            try
            {
                foreach (Contato contato in contatos)
                {
                    if (contato.Telefone == null) throw new Exception("TELEFONE NULO NO CADASTRO");
                    var estabelecimentos = await _unitOfWork.repositorioEstabelecimento
                                                            .GetAllEstabelecimentoPorContatoQuePossuiCompetenciaVigente(contato.Telefone);
                    foreach(Estabelecimento estabelecimento in estabelecimentos)
                    {
                        var estabelecimentoPdf = await _unitOfWork.repositorioEstabelecimento.FindEstabelecimentoPorCnpj(estabelecimento.Cnpj);
                        if (estabelecimentoPdf == null) throw new Exception("ESTABELECIMENTO COM DADOS CORROMPIDOS");
                        await GerarProcessamentoPorEstabelecimento(estabelecimentoPdf, competencia,
                                                               contentRootPath, caminhoApp);
                        var telefone = contato.Telefone ?? "";
                        var mensagem = new EnvioMensagemMensal(competencia, estabelecimentoPdf.Cnpj, telefone);
                        await _unitOfWork.GetRepository<EnvioMensagemMensal>().AddAsync(mensagem);
                        await _unitOfWork.CommitAsync();
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
            try
            {
                if (estabelecimento.ExtratoVendas.Count > 0)
                {
                    var posicaoNaRede = await _unitOfWork.repositorioRede.BuscarRankingDoEstabelecimentoNaRede(competencia, estabelecimento);
                    var arquivoPdf = $"{estabelecimento.Cnpj}-{estabelecimento.RazaoSocial?.Replace(" ","_")}.pdf";
                    _servicoGrafico.GerarGrafico(estabelecimento, contentRootPath);
                    await _servicoGrafico.GerarArquivoPdf(estabelecimento, arquivoPdf,
                                                        posicaoNaRede, contentRootPath, caminhoApp);
                    var estabelecimentoUpdate = await _unitOfWork.repositorioEstabelecimento.GetByIdStringAsync(estabelecimento.Cnpj);
                    if (estabelecimentoUpdate != null)
                    {
                        estabelecimentoUpdate.UltimoPdfGerado = $"{arquivoPdf}";
                        _unitOfWork.repositorioEstabelecimento.Update(estabelecimentoUpdate);
                        await _unitOfWork.CommitAsync();
                    }
                }     
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro {ex.Message}");
            }
                                           
        }
    }
}
