using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers
{
    [ApiController]
    [Route("Api/v1/[Controller]")]
    public class ServicosController : ControllerBase
    {
        private readonly IServicoContato _servicoContato;

        public ServicosController(IServicoContato servicoContato)
        {
            _servicoContato = servicoContato;
        }

        [HttpPost]
        [Route("Contato/Aceite/{Token}")]
        public async Task<ActionResult> AceiteContato([FromQuery] Guid Token){
            var contato = await _servicoContato.FindByPredicate(x => x.Token == Token);
            if (contato == null){
                return BadRequest();
            }
            contato.DataAceite = DateTime.Now;
            contato.AceitaMensagem = true;
            _servicoContato.Update(contato);
            return Ok();
        }

        [HttpPost]
        [Route("Contato/Recusa/{Token}")]
        public async Task<ActionResult> RecusaContato([FromQuery] Guid Token){
            var contato = await _servicoContato.FindByPredicate(x=>x.Token == Token);
            if (contato == null){
                return BadRequest();
            }
            contato.DataRecusa = DateTime.Now;
            contato.RecusaMensagem = true;
            contato.AceitaMensagem = false;
            _servicoContato.Update(contato);
            return Ok();
        }
    }
}