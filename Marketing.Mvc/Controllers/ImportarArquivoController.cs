using Marketing.Application.DTOs;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marketinf.Mvc.Controllers
{
    public class ImportarArquivoController : Controller
    {
        private readonly IServicoImportarPlanilha _servicoImportarPlanilha;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly ILogger<ImportarArquivoController> _logger;


        public ImportarArquivoController(IServicoImportarPlanilha servicoImportarPlanilha, IWebHostEnvironment webHostEnviroment,
                                         ILogger<ImportarArquivoController> logger, IUnitOfWork unitOfWork)
        {
            _servicoImportarPlanilha = servicoImportarPlanilha;
            _webHostEnviroment = webHostEnviroment;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> ImportarPlanilhaTratada(string? erro = null, string? sucesso = null)
        {
            var redes = await _unitOfWork.GetRepository<Rede>().GetAll();;
            var selectList = new SelectList(redes, "Nome", "Nome");
            ViewBag.Redes = selectList;
            ViewData["Erro"] = erro;
            ViewData["OK"] = sucesso;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportarPlanilhaTratada(ImportarIncidenciaDto importarIncidenciaDto)
        {
            try
            {
                if (importarIncidenciaDto.Rede == null) return RedirectToAction("Index", new { erro = "Selecione a Rede" });
                if (importarIncidenciaDto.arquivoEnviado == null || importarIncidenciaDto.arquivoEnviado.Length == 0) return RedirectToAction("ImportarPlanilhaTratada", new { erro = "Nenhum arquivo foi selecionado!" });
                var extensao = Path.GetExtension(importarIncidenciaDto.arquivoEnviado.FileName);
                if (extensao.ToLower() != ".xlsx") return RedirectToAction("ImportarPlanilhaTratada", new { erro = "O arquivo selecionado não é uma planilha Excel!" });
                var nomeArquivo = importarIncidenciaDto.arquivoEnviado.FileName.Replace(extensao, "").Split("_");
                if (nomeArquivo[2] != importarIncidenciaDto.Rede) return RedirectToAction("ImportarPlanilhaTratada",
                        new { erro = $"O arquivo anexado contém dados da rede {nomeArquivo[2]}. É necessário selecionar a rede correspondente!" });
                var sucesso = await _servicoImportarPlanilha.ImportarPlanilhaNovo(importarIncidenciaDto.arquivoEnviado,
                                            importarIncidenciaDto.Rede, _webHostEnviroment.ContentRootPath);
                return RedirectToAction("ImportarPlanilhaTratada", new { sucesso = "Arquivo importado com sucesso" });
            }
            catch (System.Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return RedirectToAction("ImportarPlanilhaTratada", new { erro = "Erro importando arquivo! Contate o administrador do sistema para detalhes." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ImportarPlanilhaCoca(string? erro = null, string? sucesso = null)
        {
            var templates = await _unitOfWork.GetRepository<TemplateImportarPlanilha>().GetAll();
            var selectList = new SelectList(templates, "Id", "Template");
            ViewBag.Templates = selectList;
            ViewData["Erro"] = erro;
            ViewData["OK"] = sucesso;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportarPlanilhaCoca(ImportarIncidenciaCocaDto importarIncidenciaCocaDto)
        {
            try
            {
                if (importarIncidenciaCocaDto.Template == 0) return RedirectToAction("ImportarPlanilhaCoca", new { erro = "Selecione o Template" });
                if (importarIncidenciaCocaDto.arquivoEnviado == null || importarIncidenciaCocaDto.arquivoEnviado.Length == 0) return RedirectToAction("ImportarPlanilhaTratada", new { erro = "Nenhum arquivo foi selecionado!" });
                var extensao = Path.GetExtension(importarIncidenciaCocaDto.arquivoEnviado.FileName);
                if (extensao.ToLower() != ".xlsx") return RedirectToAction("ImportarPlanilhaCoca", new { erro = "O arquivo selecionado não é uma planilha Excel!" });
                var nomeArquivo = importarIncidenciaCocaDto.arquivoEnviado.FileName.Replace(extensao, "").Split("_");
                
                var response = await _servicoImportarPlanilha.ImportarPlanilhaOriginal(importarIncidenciaCocaDto.arquivoEnviado,
                                            importarIncidenciaCocaDto.Template.ToString(), _webHostEnviroment.ContentRootPath);
                
                return RedirectToAction("ImportarPlanilhaTratada", new { sucesso = "Arquivo importado com sucesso" });
            }
            catch (System.Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return RedirectToAction("ImportarPlanilhaTratada", new { erro = "Erro importando arquivo! Contate o administrador do sistema para detalhes." });
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