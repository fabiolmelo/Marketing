using Marketing.Application.DTOs;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers
{
    public class FechamentoController : Controller
    {
        private readonly IServicoProcessamentoMensal _servicoProcessamentoMensal;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServicoEnvioMensagemMensal _servicoEnvioMensagemMensal;
        private readonly ILogger<FechamentoController> _logger;

        public FechamentoController(IServicoProcessamentoMensal servicoProcessamentoMensal,
                                                IWebHostEnvironment webHostEnviroment,
                                                IConfiguration configuration,
                                                ILogger<FechamentoController> logger,
                                                IServicoEnvioMensagemMensal servicoEnvioMensagemMensal,
                                                IRepositorioMensagem repositorioMensagem,
                                                IUnitOfWork unitOfWork)
        {
            _servicoProcessamentoMensal = servicoProcessamentoMensal;
            _webHostEnviroment = webHostEnviroment;
            _configuration = configuration;
            _logger = logger;
            _servicoEnvioMensagemMensal = servicoEnvioMensagemMensal;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index(string? erro = null, string? sucesso = null)
        {
            try
            {
                var competenciaVigente = await _unitOfWork.repositorioExtratoVendas.BuscarCompetenciaVigente();
                if (competenciaVigente != null) ViewBag.CompetenciaVigente = competenciaVigente?.ToString("yyyy-MM");
                ViewData["Erro"] = erro;
                ViewData["OK"] = sucesso;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return View();
            }
            
        }

        [HttpPost]
        public IActionResult Gerar(ProcessamentoMensalDto processamentoMensalDto)
        {
            try
            {
                var caminhoApp = _configuration["Aplicacao:Url"];
                if (caminhoApp == null) throw new Exception("Arquivo de configuração inválido");
                var sucesso = _servicoProcessamentoMensal.GerarProcessamentoMensal(
                                processamentoMensalDto.Competencia,
                                _webHostEnviroment.ContentRootPath, caminhoApp);
                _logger.LogInformation($"Fechamento processado para a competencia {processamentoMensalDto.Competencia.ToShortDateString()}");
                return RedirectToAction("Index",
                    new { sucesso = "Processamento efetuado com sucesso!" });
            }
            catch (System.Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return RedirectToAction("Index",
                    new { erro = "Erro. Processamento não efetuado!" });
            }
        }

        [HttpGet("Fechamento/Download/{cnpj}/{telefone}")]
        public async Task<IActionResult> Download(string cnpj, string telefone)
        {
            try
            {
                var contato = await _unitOfWork.repositorioContato.GetByIdStringAsync(telefone);
                var estabelecimento = await _unitOfWork.repositorioEstabelecimento.GetByIdStringAsync(cnpj);
                var competenciaVigente = await _unitOfWork.repositorioExtratoVendas.BuscarCompetenciaVigente();
                if (estabelecimento == null || estabelecimento.UltimoPdfGerado == null || contato == null)
                {
                    throw new Exception($"Estabelecimento{estabelecimento?.Cnpj}/Contato {contato?.Telefone}/Competencia não localizada!");
                }
                var mensagemEnvio = await _unitOfWork.repositorioEnvioMensagemMensal.GetByCompetenciaEstabelecimentoContato(competenciaVigente,
                                            estabelecimento.Cnpj, contato.Telefone);
                if (mensagemEnvio == null || mensagemEnvio.MensagemId == null)
                {
                    throw new Exception("Mensagem enviada ao contato não localizada!");
                }
                var mensagemItem = new MensagemItem(mensagemEnvio.MensagemId, DateTime.Now, MensagemStatus.ClicouLink);
                await _unitOfWork.GetRepository<MensagemItem>().AddAsync(mensagemItem);
                await _unitOfWork.CommitAsync();
                var pathRoot = Path.Combine(_webHostEnviroment.ContentRootPath, "DadosApp", "images", estabelecimento.UltimoPdfGerado);
                byte[] fileBytes = System.IO.File.ReadAllBytes(pathRoot);
                return File(fileBytes, "application/pdf", $"{estabelecimento.UltimoPdfGerado}");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return BadRequest();
            }
            
        }
    }
}
