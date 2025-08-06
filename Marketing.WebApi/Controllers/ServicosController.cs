using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers
{
    public static class ServicosController
    {

        public static void AddApiServicosController(this WebApplication app)
        {
            app.MapGet("contato/aceite/{token}",
                async ([FromServices] IServicoContato _servicoContato,
                       [FromRoute] Guid token) =>
            {
                var contato = await _servicoContato.FindByPredicate(x => x.Token == token);
                if (contato == null)
                {
                    return Results.BadRequest();
                }
                contato.DataAceite = DateTime.Now;
                contato.AceitaMensagem = true;
                contato.DataRecusa = DateTime.MinValue;
                contato.RecusaMensagem = false;
                _servicoContato.Update(contato);
                return Results.Ok();
            })
            .WithName("AceiteContato")
            .WithOpenApi();
            
            app.MapGet("contato/recusa/{token}",
                async ([FromServices] IServicoContato _servicoContato,
                       [FromRoute] Guid token) =>
            {
                var contato = await _servicoContato.FindByPredicate(x => x.Token == token);
                if (contato == null)
                {
                    return Results.BadRequest("");
                }
                contato.DataRecusa = DateTime.Now;
                contato.RecusaMensagem = true;
                contato.AceitaMensagem = false;
                contato.DataAceite = DateTime.MinValue;
                _servicoContato.Update(contato);
                return Results.Ok();
            })
            .WithName("RecusaContato")
            .WithOpenApi();

        }
    }
}