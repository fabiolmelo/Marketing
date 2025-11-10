using Marketing.Application.DTOs;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marketinf.Mvc.Controllers
{
    public class ImportarArquivoController : Controller
    {
        private readonly IServicoImportarPlanilha _servicoImportarPlanilha;
        private readonly IServicoRede _servicoRede;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly ILogger<ImportarArquivoController> _logger;


        public ImportarArquivoController(IServicoImportarPlanilha servicoImportarPlanilha, IServicoRede servicoRede, IWebHostEnvironment webHostEnviroment, ILogger<ImportarArquivoController> logger)
        {
            _servicoImportarPlanilha = servicoImportarPlanilha;
            _servicoRede = servicoRede;
            _webHostEnviroment = webHostEnviroment;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? erro = null, string? sucesso = null)
        {
            var redes = await _servicoRede.GetAllRedesAsync(1, 999999);
            var selectList = new SelectList(redes.Dados, "Nome", "Nome");
            ViewBag.Redes = selectList;
            ViewData["Erro"] = erro;
            ViewData["OK"] = sucesso;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ImportarIncidenciaDto importarIncidenciaDto)
        {
            try
            {
                if (importarIncidenciaDto.Rede == null) return RedirectToAction("Index", new { erro = "Selecione a Rede" });
                if (importarIncidenciaDto.arquivoEnviado == null || importarIncidenciaDto.arquivoEnviado.Length == 0) return RedirectToAction("Index", new { erro = "Nenhum arquivo foi selecionado!" });
                var extensao = Path.GetExtension(importarIncidenciaDto.arquivoEnviado.FileName);
                if (extensao.ToLower() != ".xlsx") return RedirectToAction("Index", new { erro = "O arquivo selecionado não é uma planilha Excel!" });
                var nomeArquivo = importarIncidenciaDto.arquivoEnviado.FileName.Replace(extensao, "").Split("_");
                if (nomeArquivo[2] != importarIncidenciaDto.Rede) return RedirectToAction("Index",
                        new { erro = $"O arquivo anexado contém dados da rede {nomeArquivo[2]}. É necessário selecionar a rede correspondente!" });
                var sucesso = await _servicoImportarPlanilha.ImportarPlanilha(importarIncidenciaDto.arquivoEnviado,
                                            importarIncidenciaDto.Rede, _webHostEnviroment.ContentRootPath);
                return RedirectToAction("Index", new { sucesso = "Arquivo importado com sucesso" });
            }
            catch (System.Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return RedirectToAction("Index", new { sucesso = "Erro importando arquivo! Contate o administrador do sistema para detalhes." });
            }
            
        }

        [HttpGet]
        public IActionResult ImportarContato(string? erro = null, string? sucesso = null)
        {
            ViewData["Erro"] = erro;
            ViewData["OK"] = sucesso;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportarContato(IFormFile arquivoEnviado)
        {
            try
            {
                if (arquivoEnviado == null || arquivoEnviado.Length == 0) return RedirectToAction("Index", new { erro = "Nenhum arquivo foi selecionado!" });
                var extensao = Path.GetExtension(arquivoEnviado.FileName);
                if (extensao.ToLower() != ".xlsx") return RedirectToAction("Index", new { erro = "O arquivo selecionado não é uma planilha Excel!" });
                var sucesso = await _servicoImportarPlanilha.ImportarContato(arquivoEnviado, _webHostEnviroment.ContentRootPath);
                return RedirectToAction("ImportarContato", new { sucesso = "Contatos atualizados com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return RedirectToAction("Index", new { erro = "Erro importando arquivo! Contate o administrador do sistema para detalhes." });
            }
        }
    }
}