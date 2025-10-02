using System.Text.Json;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Mvc.Controllers
{
    public class EnvioMensagemController : Controller
    {
        private readonly IServicoExtratoVendas _servicoExtratoVendas;
        private readonly IServicoEnvioMensagemMensal _servicoEnvioMensagemMensal;
        private readonly IServicoMeta _servicoMeta;
        private readonly IServicoContato _servicoContato;
        private readonly IServicoEstabelecimento _servicoEstabelecimento;
        private readonly IConfiguration _configuration;

        public EnvioMensagemController(IServicoExtratoVendas servicoExtratoVendas, IServicoEnvioMensagemMensal servicoEnvioMensagemMensal, IConfiguration configuration, IServicoMeta servicoMeta, IServicoContato servicoContato, IServicoEstabelecimento servicoEstabelecimento)
        {
            _servicoExtratoVendas = servicoExtratoVendas;
            _servicoEnvioMensagemMensal = servicoEnvioMensagemMensal;
            _configuration = configuration;
            _servicoMeta = servicoMeta;
            _servicoContato = servicoContato;
            _servicoEstabelecimento = servicoEstabelecimento;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int pageNumber = 1, int pageSize = 5, bool pendentes = true)
        {
            var enviosPendentes = await _servicoEnvioMensagemMensal
                                            .BuscarMensagensNaoEnviadas(pageNumber, pageSize, pendentes);
            return View(enviosPendentes);
        }

        [HttpPost]
        public async Task<ActionResult> EnviarTodos()
        {
            var caminhoApp = _configuration["Aplicacao:Url"];
            if (caminhoApp == null) throw new Exception("Arquivo de configuração inválido");
            var enviosPendentes = await _servicoEnvioMensagemMensal.BuscarTodasMensagensNaoEnviadas();
            foreach (var envioPendente in enviosPendentes)
            {
                var envio = await _servicoEnvioMensagemMensal.GetByIdChaveComposta3(envioPendente.Competencia,
                                    envioPendente.ContatoTelefone, envioPendente.EstabelecimentoCnpj);
                if (envio != null)
                {
                    var contato = await _servicoContato.GetByIdStringAsync(envio.ContatoTelefone);
                    var estabelecimento = await _servicoEstabelecimento.GetByIdStringAsync(envio.EstabelecimentoCnpj);

                    if (contato != null && estabelecimento != null)
                    {
                        ServicoExtratoResponseDto response = await _servicoMeta.EnviarExtrato(contato, estabelecimento, caminhoApp);
                        if (response.IsSuccessStatusCode)
                        {
                            WhatsAppResponseResult? json = JsonSerializer.Deserialize<WhatsAppResponseResult>(response.Response, JsonSerializerOptions.Default);
                            if (json != null && contato.Telefone != null)
                            {
                                foreach (Message message in json.messages)
                                {
                                    var mensagemId = message.id;
                                    if (mensagemId != null)
                                    {
                                        var mensagem = new Mensagem(mensagemId);
                                        mensagem.AdicionarEvento(MensagemStatus.sent);
                                        envio.Mensagem = mensagem;
                                        _servicoEnvioMensagemMensal.Update(envio);
                                        await _servicoEnvioMensagemMensal.CommitAsync();
                                    }
                                }
                            }
                        }
                        else
                        {
                            var mensagem = new Mensagem(Guid.NewGuid().ToString());
                            mensagem.AdicionarEvento(MensagemStatus.failed);
                            _servicoEnvioMensagemMensal.Update(envio);
                            await _servicoEnvioMensagemMensal.CommitAsync();
                        }
                     }
                }
            }
            return RedirectToAction("Index");
        }
    }
}