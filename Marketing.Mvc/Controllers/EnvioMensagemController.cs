using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers
{
    public class EnvioMensagemController : Controller
    {
        private readonly IServicoExtratoVendas _servicoExtratoVendas;
        private readonly IServicoEnvioMensagemMensal _servicoEnvioMensagemMensal;

        public EnvioMensagemController(IServicoExtratoVendas servicoExtratoVendas)
        {
            _servicoExtratoVendas = servicoExtratoVendas;
        }

        [HttpGet]
        public async Task<ActionResult> Index(DateTime? Competencia)
        {
            var competencia = Competencia ?? await _servicoExtratoVendas.BuscarCompetenciaVigente();
            var enviosPendentes = await _servicoEnvioMensagemMensal.BuscarMensagensNaoEnviadas(competencia);
            return View(enviosPendentes);
        }
    }
}