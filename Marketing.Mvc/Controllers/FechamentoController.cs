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

        public FechamentoController(IServicoProcessamentoMensal servicoProcessamentoMensal,
                                                IWebHostEnvironment webHostEnviroment,
                                                IConfiguration configuration)
        {
            _servicoProcessamentoMensal = servicoProcessamentoMensal;
            _webHostEnviroment = webHostEnviroment;
            _configuration = configuration;
        }

        public ActionResult Index(bool? gerou)
        {
            ViewBag.Gerou = gerou;
            return View();
        }

        [HttpPost]
        public IActionResult Gerar(ProcessamentoMensalDto processamentoMensalDto)
        {
            try
            {
                var caminhoApp = _configuration["Aplicacao:Url"];
                if (caminhoApp == null)
                {
                    throw new Exception("Arquivo de configuração inválido");
                }
                var sucesso = _servicoProcessamentoMensal.GerarProcessamentoMensal(
                                processamentoMensalDto.Competencia,
                                _webHostEnviroment.ContentRootPath, caminhoApp);
                return RedirectToAction("Index",
                    new { gerou = true });
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index",
                    new { gerou = false });
            } 
        }
    }
}
