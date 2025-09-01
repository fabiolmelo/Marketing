using System.Diagnostics;
using System.Text.Json;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

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

        //var contato = new Contato("5511976459155"); //Fone Priscila
        var contato = new Contato("5511977515914"); 
        contato.Nome = "Mob Creative Teste Ltda";
        var result = await _servicoMeta.EnviarExtrato(contato, "21936.pdf");
        if (result.IsSuccessStatusCode)
        {
            var json = JsonSerializer.Deserialize<WhatsAppMessageTemplate>(result.Response);
        }
        else
        {
            var json = JsonSerializer.Deserialize<WhatsAppResponseError>(result.Response);
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
