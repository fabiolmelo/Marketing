using Marketing.Domain.Entidades.Meta;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.WebApi.Controllers
{
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public WebhookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> HandleWebhook([FromBody] WhatsAppWebhookPayload payload)
        {
            if (payload == null)
            {
                return BadRequest(); 
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
                            // ...
                        }
                    }
                }
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult VerifyWebhook(string hubMode, string hubChallenge, string hubVerifyToken)
        {
            var tokenVerificar = _configuration["Meta:TokenVerificacaoApi"];
            if (hubMode == "subscribe" && hubVerifyToken == tokenVerificar)
            {
                return Ok(hubChallenge);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}