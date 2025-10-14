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
            ViewBag.Erro = erro;
            ViewBag.Sucesso = sucesso;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile arquivoEnviado)
        {
            if (arquivoEnviado == null) return RedirectToAction("Index", new { erro = "Arquivo não selecionado" }); 
            var sucesso = await _servicoImportarPlanilha.ImportarPlanilha(arquivoEnviado);
            if (!sucesso) return BadRequest();
            return RedirectToAction("Index", new { erro = "Arquivo não selecionado" });
        }
    }
}