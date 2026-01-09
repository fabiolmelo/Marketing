using System.Text.Json;
using Marketing.Domain.Entidades;
using Marketing.Domain.Entidades.Meta;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Servicos;
using Marketing.Infraestrutura.Contexto;
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
        private readonly DataContextMySql _dataContextMySql;
        private readonly ILogger<ApiServicoController> _logger;

        public ApiServicoController(IServicoMeta servicoMeta, IUnitOfWork unitOfWork, IConfiguration configuration, DataContextMySql dataContextMySql, ILogger<ApiServicoController> logger)
        {
            _servicoMeta = servicoMeta;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _dataContextMySql = dataContextMySql;
            _logger = logger;
        }

        [HttpGet]
        [Route("webhooks")]
        public async Task<IActionResult> Webhook([FromServices] IConfiguration _configuration,
                                                 [FromQuery(Name = "hub.mode")] string hubMode,
                                                 [FromQuery(Name = "hub.challenge")] string hubChallenge,
                                                 [FromQuery(Name = "hub.verify_token")] string hubVerifyToken)
        {
            return Ok(hubChallenge);
            // var tokenVerify = _configuration["Meta:TokemVerify"];
            // if (hubMode == "subcribe" && tokenVerify == hubVerifyToken)
            // {
            //     return Ok(hubChallenge);
            // }
            // else
            // {
            //     return Unauthorized();
            // }
        }
        
        [HttpPost]
        [Route("webhooks")]
        [Consumes("application/json")]
        public async Task<IActionResult> WebHook([FromBody] WhatsAppWebhookPayload payload)
        {
            try
            {
                if (payload == null)
                {
                    return BadRequest();
                }
                // Processar o payload...
                if (payload != null)
                {
                    var jsonSerialize = JsonSerializer.Serialize(payload);
                    _logger.LogWarning(jsonSerialize);
                    var metaWebhookResponse = new MetaWebhookResponse(jsonSerialize);
                    await _unitOfWork.GetRepository<MetaWebhookResponse>().AddAsync(metaWebhookResponse);
                    await _unitOfWork.CommitAsync(); 
                    await _dataContextMySql.MetaWebhookResponses.AddAsync(metaWebhookResponse);
                    await _dataContextMySql.SaveChangesAsync();

                    foreach (var entry in payload.Entry)
                    {
                        foreach (var change in entry.Changes)
                        {
                            foreach (var status in change.Value.Statuses)
                            {
                                // Processar o status...
                                var statusId = status.Id;
                                var statusStatus = status.Status;

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
                                            case "failed":
                                                mensagemItem = new MensagemItem(mensagem.Id, DateTime.Now, 
                                                                        MensagemStatus.FAILED, status.ErrorToString());
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
            catch (System.Exception ex)
            {
                _logger.LogCritical(ex.Message);
                _logger.LogCritical(JsonSerializer.Serialize(payload));
            }
            return Ok();
        }
    }
}