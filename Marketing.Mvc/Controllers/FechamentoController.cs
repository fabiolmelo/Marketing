using Marketing.Application.DTOs;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers
{
    public class FechamentoController : Controller
    {
        private readonly IServicoProcessamentoMensal _servicoProcessamentoMensal;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IConfiguration _configuration;
        private readonly IServicoEstabelecimento _servicoEstabelecimento;
        private readonly IServicoExtratoVendas _servicoExtratoVendas;

        public FechamentoController(IServicoProcessamentoMensal servicoProcessamentoMensal,
                                                IWebHostEnvironment webHostEnviroment,
                                                IConfiguration configuration,
                                                IServicoEstabelecimento servicoEstabelecimento,
                                                IServicoExtratoVendas servicoExtratoVendas)
        {
            _servicoProcessamentoMensal = servicoProcessamentoMensal;
            _webHostEnviroment = webHostEnviroment;
            _configuration = configuration;
            _servicoEstabelecimento = servicoEstabelecimento;
            _servicoExtratoVendas = servicoExtratoVendas;
        }

        public async Task<ActionResult> Index(string? erro = null, string? sucesso = null)
        {
            var competenciaVigente = await _servicoExtratoVendas.BuscarCompetenciaVigente();
            ViewBag.CompetenciaVigente = competenciaVigente.ToString("yyyy-MM");
            ViewData["Erro"] = erro;
            ViewData["OK"] = sucesso;
            return View();
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
                return RedirectToAction("Index",
                    new { sucesso = "Processamento efetuado com sucesso!" });
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index",
                    new { erro = "Erro. Processamento não efetuado!" });
            }
        }

        [HttpGet("Fechamento/Download/{cnpj}")]
        public async Task<IActionResult> Download(string cnpj)
        {
            var estabelecimento = await _servicoEstabelecimento.GetByIdStringAsync(cnpj);
            if (estabelecimento == null || estabelecimento.UltimoPdfGerado == null) return BadRequest();
            var pathRoot = Path.Combine(_webHostEnviroment.ContentRootPath, "DadosApp", "images", estabelecimento.UltimoPdfGerado);
            byte[] fileBytes = System.IO.File.ReadAllBytes(pathRoot);
            return File(fileBytes, "application/pdf", $"{estabelecimento.UltimoPdfGerado}");
        }
    }
}
