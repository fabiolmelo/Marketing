using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;


namespace Marketing.Application.Servicos
{
    public class ServicoMeta : IServicoMeta
    {
        private readonly HttpClient _httpClient;

        public ServicoMeta(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MetaHttpClient");
        }

        public async Task<bool> EnviarSolitacaoAceiteContatoASync(Contato contato)
        {
            WhatsAppMessageTemplate request = new WhatsAppMessageTemplate("5511977515914", "teste_link", "pt_BR");
            Component body = new Component("body");
            body.parameters.Add(new Parameter("text", "Fabio"));
            body.parameters.Add(new Parameter("text", "Melo"));
            Component footer = new Component("button");
            footer.parameters.Add(new Parameter("url", "https://www.mob.com.br/teste/"));
            footer.parameters.Add(new Parameter("text", "21196350000145"));
            request.template.components.Add(body);
            request.template.components.Add(footer);
            var json = JsonSerializer.Serialize<WhatsAppMessageTemplate>(request);
            var response = await _httpClient.PostAsJsonAsync("", json);
            return response.IsSuccessStatusCode;
        }
        
        public Task<bool> EnviarTesteASync(Contato contato)
        {
            throw new NotImplementedException();
        }
    }
}