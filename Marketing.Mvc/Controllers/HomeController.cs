using System.Diagnostics;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IServicoMeta _servicoMeta;

    public HomeController(ILogger<HomeController> logger, IServicoMeta servicoMeta)
    {
        _logger = logger;
        _servicoMeta = servicoMeta;
    }

    public async Task<IActionResult> Index()
    {
        var sucesso = await _servicoMeta.EnviarSolitacaoAceiteContatoASync(new Contato());
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
