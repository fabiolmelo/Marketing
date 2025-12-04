using System.Text.Json;
using Marketing.Domain.Entidades;
using Marketing.Domain.Entidades.Meta;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ApiServicoController : ControllerBase 
    {
        private readonly IServicoMeta _servicoMeta;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ApiServicoController(IServicoMeta servicoMeta, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _servicoMeta = servicoMeta;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        
        [HttpGet]
        [Route("webhooks")]
        public async Task<IActionResult> WebHook([FromBody] WhatsAppWebhookPayload payload)
        {
            if (payload == null)
            {
                return BadRequest();
            }
                // Processar o payload...
            if (payload != null)
            {
                var jsonSerialize = JsonSerializer.Serialize(payload);
                var metaWebhookResponse = new MetaWebhookResponse(jsonSerialize);
                await _unitOfWork.GetRepository<MetaWebhookResponse>().AddAsync(metaWebhookResponse);
                await _unitOfWork.CommitAsync(); 

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
                                    var mensagem = await _unitOfWork.repositorioMensagem.FindByPredicate(x=>x.MetaMensagemId == statusId);
                                    if (mensagem != null)
                                    {
                                        MensagemItem mensagemItem;
                                        switch (statusStatus)
                                        {
                                            case "sent":
                                                mensagemItem = new MensagemItem(mensagem.Id, DateTime.Now, MensagemStatus.SENT);
                                                break;
                                            case "delivered":
                                                mensagemItem = new MensagemItem(mensagem.Id, DateTime.Now, MensagemStatus.DELIVERED);
                                                break;
                                            case "read":
                                                mensagemItem = new MensagemItem(mensagem.Id, DateTime.Now, MensagemStatus.READ);
                                                break;
                                            default:
                                                mensagemItem = new MensagemItem(mensagem.Id, DateTime.Now, MensagemStatus.FAILED);
                                                break;
                                        }    
                                        if (mensagemItem != null)
                                        {
                                            await _unitOfWork.GetRepository<MensagemItem>().AddAsync(mensagemItem);
                                            await _unitOfWork.CommitAsync();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Ok();
        }

        
        [HttpPost]
        [Route("webhooks")]
        public async Task<IActionResult> Webhook([FromQuery(Name = "hub.mode")] string? hubMode,
                                               [FromQuery(Name = "hub.challenge")] string? hubChallenge,
                                               [FromQuery(Name = "hub.verify_token")] string? hubVerifyToken)
        {
            return Ok(hubChallenge);
        }
        
    }
}