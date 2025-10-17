using Marketing.Domain.Entidades;
using Marketing.Domain.Entidades.Meta;
using Marketing.Domain.Interfaces.IUnitOfWork;
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

            
            
            app.MapPost("HandleWebhook",
                async ([FromServices] IServicoMeta _servicoMeta,
                       [FromServices] IUnitOfWork _unitOfWork,
                       [FromBody] WhatsAppWebhookPayload payload) =>
            {
                if (payload == null)
                {
                    return Results.BadRequest();
                }
                // Processar o payload...
                foreach (var entry in payload.Entry)
                {
                    foreach (var change in entry.Changes)
                    {
                        if (change.Field == "messages")
                        {
                            foreach (var message in change.Value.Messaging.Messages)
                            {
                                // Processar a mensagem...
                                var texto = message.Text.Body;
                                // ...
                            }
                        }
                        else if (change.Field == "statuses")
                        {
                            foreach (var status in change.Value.Statuses.StatusesList)
                            {
                                // Processar o status...
                                var statusId = status.Id;
                                var statusStatus = status.StatusName;

                                if (statusId != null)
                                {
                                    var mensagem = await _unitOfWork.repositorioMensagem.FindByIdIncludeEventosAsync(statusId);
                                    if (mensagem != null)
                                    {
                                        switch (statusStatus)
                                        {
                                            case "sent":
                                                mensagem.AdicionarEvento(MensagemStatus.Enviado);
                                                break;
                                            case "delivered":
                                                mensagem.AdicionarEvento(MensagemStatus.Entregue);
                                                break;
                                            case "read":
                                                mensagem.AdicionarEvento(MensagemStatus.Lida );
                                                break;
                                            default:
                                                mensagem.AdicionarEvento(MensagemStatus.Falha);
                                                break;
                                        }    
                                    }
                                    
                                }
                            }
                        }
                    }
                }
                return Results.Ok();
            })
            .WithName("HandleWebhook")
            .WithOpenApi();

        app.MapPost("VerifyWebhook",
                async([FromServices] IServicoMeta _servicoMeta,
                      [FromServices] IConfiguration _configuration,
                      [FromBody] WhatsAppVerify whatsAppVerify) =>
                {
                    var tokenVerificar = _configuration["Meta:TokenVerificacaoApi"];
                    if (whatsAppVerify.hubMode == "subscribe" && whatsAppVerify.hubVerifyToken == tokenVerificar)
                    {
                        return Results.Ok(whatsAppVerify.hubChallenge);
                    }
                    else
                    {
                        return Results.BadRequest();
                    }
                    })
                .WithName("VerifyWebhook")
                .WithOpenApi();
        }
    }
}