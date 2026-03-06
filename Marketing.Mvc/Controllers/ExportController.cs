using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Marketing.Mvc.Controllers
{
    [Authorize(Roles = "Root")]
    public class ExportController  : ControllerBase 
    {
        private readonly IServicoExport _servicoExport;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public ExportController(IServicoExport servicoExport, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnviroment)
        {
            _servicoExport = servicoExport;
            _unitOfWork = unitOfWork;
            _webHostEnviroment = webHostEnviroment;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pathRoot = Path.Combine(_webHostEnviroment.ContentRootPath, "DadosApp", "Relatorio", "BaseV1.xlsx");
            var fechamentos = await _servicoExport.GerarFechamentoV1(pathRoot);
            var fileBytes = _servicoExport.ExportarFechamentoV1(fechamentos);
            return File(fileBytes, "application/zip", $"IncidenciaV1.zip");
        }
    }
}