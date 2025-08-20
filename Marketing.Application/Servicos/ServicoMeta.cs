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
            WhatsAppMessageTemplate requestBody = new WhatsAppMessageTemplate("5511977515914", "teste_link", "pt_BR");
            
            var bodyComponent = new Component("body");
            bodyComponent.parameters.Add(new Parameter("text", "valor da variável 1"));
            bodyComponent.parameters.Add(new Parameter("text", "valor da variável 2"));
            requestBody.template.components.Add(bodyComponent);

            var buttonComponent = new Component("button");
            buttonComponent.sub_type = "url";
            buttonComponent.index = "0";
            buttonComponent.parameters.Add(new Parameter("text", "21196350000145"));
            requestBody.template.components.Add(buttonComponent);
            //request.Headers.Add("Content-Type", "text/javascript; charset=UTF-8");
            var response = await _httpClient.PostAsJsonAsync<WhatsAppMessageTemplate>("", requestBody, JsonSerializerOptions.Default);
            return response.IsSuccessStatusCode;
        }
        
        public Task<bool> EnviarTesteASync(Contato contato)
        {
            throw new NotImplementedException();
        }
    }
}