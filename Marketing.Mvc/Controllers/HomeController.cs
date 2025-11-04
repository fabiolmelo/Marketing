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
    private readonly IServicoArquivos _servicoArquivos;
    private readonly IServicoEnvioMensagemMensal _servicoEnvioMensagemMensal;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnviroment, IServicoSeed servicoSeed, IServicoEnvioMensagemMensal servicoEnvioMensagemMensal, IServicoArquivos servicoArquivos)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _webHostEnviroment = webHostEnviroment;
        _servicoSeed = servicoSeed;
        _servicoEnvioMensagemMensal = servicoEnvioMensagemMensal;
        _servicoArquivos = servicoArquivos;
    }


    public async Task<IActionResult> Index(string? erro = null, string? sucesso = null)
    {
        ViewData["Erro"] = erro;
        ViewData["OK"] = sucesso;
        await _servicoSeed.SeedConfiguracoesApp();
        await _servicoSeed.SeedRedes();
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
    public async Task<IActionResult> Download()
    {
        try
        {
            var competencia = await _unitOfWork.repositorioExtratoVendas.BuscarCompetenciaVigente();
            var mensagens = new List<Mensagem>();

            if (competencia != null)
            {
                mensagens = await _unitOfWork.repositorioMensagem.GetAllMensagemsAsync(competencia);
                var resumo = await _unitOfWork.repositorioMensagem.BuscaResumoMensagemPorCompetencia(competencia);
                var pathRoot = Path.Combine(_webHostEnviroment.ContentRootPath, "DadosApp", "Relatorio", "BaseNova.xlsx");
                var pathRootBase = Path.Combine(_webHostEnviroment.ContentRootPath, "DadosApp", "Relatorio",
                                                $"Relatório de Envio de Extratos de Incidência_{competencia?.ToString("MMMMyyyy")}.xlsx");
                _servicoArquivos.GerarRelatorioEnvios(pathRoot, mensagens, resumo, pathRootBase);
                byte[] fileBytes = System.IO.File.ReadAllBytes(pathRootBase);
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            $"Relatório de Envio de Extratos de Incidência_{competencia?.ToString("MMMMyyyy")}.xlsx");
            }
            return RedirectToAction("Index", new {erro = "Não existe nenhum fechamento realizado!"});
        }
        catch (System.Exception)
        {
            return RedirectToAction("Index", new {erro = "Erro ao buscar relatório de incidências!"});
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
