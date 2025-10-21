using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Marketinf.Mvc.Controllers
{
    public class ImportarArquivoController : Controller
    {
        private readonly IServicoImportarPlanilha _servicoImportarPlanilha;

        public ImportarArquivoController(IServicoImportarPlanilha servicoImportarPlanilha)
        {
            _servicoImportarPlanilha = servicoImportarPlanilha;
        }

        public IActionResult Index(string? erro = null, string? sucesso = null)
        {
            ViewData["Erro"] = erro;
            ViewData["OK"] = sucesso;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile arquivoEnviado)
        {
            if (arquivoEnviado == null || arquivoEnviado.Length == 0) return RedirectToAction("Index", new { erro = "Nenhum arquivo foi selecionado!" });
            var extensao = Path.GetExtension(arquivoEnviado.FileName);
            if (extensao.ToLower() != ".xlsx") return RedirectToAction("Index", new { erro = "O arquivo selecionado não é uma planilha Excel!" });
            var sucesso = await _servicoImportarPlanilha.ImportarPlanilha(arquivoEnviado);
            if (!sucesso) return BadRequest();
            return RedirectToAction("Index", new { sucesso = "Arquivo importado com sucesso" });
        }
    }
}