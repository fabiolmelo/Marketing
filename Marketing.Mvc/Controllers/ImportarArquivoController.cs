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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile arquivoEnviado)
        {
            var suceesso = await _servicoImportarPlanilha.ImportarPlanilha(arquivoEnviado);
            if (!suceesso) return BadRequest();
            return View();
        }
    }
}