using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Marketing.Mvc.Controllers
{
    [Authorize(Roles = "Root")]
    [EnableRateLimiting("mvc") ]
    public class ExportController  : ControllerBase 
    {
        private readonly IServicoExport _servicoExport;
        private readonly IUnitOfWork _unitOfWork;
        public ExportController(IServicoExport servicoExport, IUnitOfWork unitOfWork)
        {
            _servicoExport = servicoExport;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fechamentos = new List<FechamentoV1>();
            var redes = await _unitOfWork.GetRepository<Rede>().GetAll();
            foreach(var rede in redes)
            {
                fechamentos.Add(await _servicoExport.GetFechamentoV1PorRede(rede.Nome));
            }
            var fileBytes = _servicoExport.ExportarFechamentoV1(fechamentos);
            return File(fileBytes, "application/zip", $"IncidenciaV1.zip");
        }
    }
}