using System.Diagnostics;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnviroment;
    private readonly IServicoSeed _servicoSeed;
    private readonly IServicoEnvioMensagemMensal _servicoEnvioMensagemMensal;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnviroment, IServicoSeed servicoSeed, IServicoEnvioMensagemMensal servicoEnvioMensagemMensal)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _webHostEnviroment = webHostEnviroment;
        _servicoSeed = servicoSeed;
        _servicoEnvioMensagemMensal = servicoEnvioMensagemMensal;
    }


    public async Task<IActionResult> Index()
    {
        await _servicoSeed.SeedConfiguracoesApp();
        var competencia = await _unitOfWork.repositorioExtratoVendas.BuscarCompetenciaVigente();
        var mensagens = new List<ResumoMensagem>(); 
        if (competencia != null)
        {
            mensagens = await _unitOfWork.repositorioMensagem.BuscaResumoMensagemPorCompetencia(competencia);
            var naoDisparados = await _unitOfWork.repositorioEnvioMensagemMensal.BuscarTodasMensagensNaoEnviadas(competencia);
            ViewData["Falhas"] = mensagens.FirstOrDefault(x => x.MensagemStatus == MensagemStatus.Falha)?.Qtd ?? 0;
            ViewData["Pendentes"] = naoDisparados.Count();
            ViewData["Disparados"] = mensagens.FirstOrDefault(x => x.MensagemStatus == MensagemStatus.Disparado)?.Qtd ?? 0;
            ViewData["Enviados"] = mensagens.FirstOrDefault(x => x.MensagemStatus == MensagemStatus.Enviado)?.Qtd ?? 0;
            ViewData["Entregues"] = mensagens.FirstOrDefault(x => x.MensagemStatus == MensagemStatus.Entregue)?.Qtd ?? 0;
            ViewData["Lidos"] = mensagens.FirstOrDefault(x => x.MensagemStatus == MensagemStatus.Lida)?.Qtd ?? 0;
            ViewData["Visualizados"] = mensagens.FirstOrDefault(x => x.MensagemStatus == MensagemStatus.ClicouLink)?.Qtd ?? 0;
        }
        return View(mensagens);
    }
    
    [HttpGet]
    public IActionResult Download()
    {
        var pathRoot = Path.Combine(_webHostEnviroment.ContentRootPath, "DadosApp", "Relatorio", "Base.xlsx");
            byte[] fileBytes = System.IO.File.ReadAllBytes(pathRoot);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Base.xlsx");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
