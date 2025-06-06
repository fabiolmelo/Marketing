using Marketing.Application.DTOs;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers
{
    public class FechamentoController : Controller
    {
        private readonly IServicoProcessamentoMensal _servicoProcessamentoMensal;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public FechamentoController(IServicoProcessamentoMensal servicoProcessamentoMensal,
                                                IWebHostEnvironment webHostEnviroment)
        {
            _servicoProcessamentoMensal = servicoProcessamentoMensal;
            _webHostEnviroment = webHostEnviroment;
        }

        public ActionResult Index(int? gerou = null)
        {
            if (gerou != null)
            {
                ViewBag.Mensagem = "Arquivo processado com sucesso!";
            }
            return View();
        }

        [HttpPost]
        public IActionResult Gerar(ProcessamentoMensalDto processamentoMensalDto)
        {
            try
            {
                var sucesso = _servicoProcessamentoMensal.GerarProcessamentoMensal(processamentoMensalDto.Competencia,
                                _webHostEnviroment.ContentRootPath);
                ViewBag.Mensagem = "Arquivo processado com sucesso!";
            }
            catch (System.Exception ex)
            {

                Console.WriteLine(ex.Message);
            } 
            return RedirectToAction("Index", new {gerou = 1});
        }
    }
}
