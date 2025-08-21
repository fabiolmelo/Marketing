using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.IdentityModel.Tokens;


namespace Marketing.Application.Servicos
{
    public class ServicoMeta : IServicoMeta
    {
        private readonly HttpClient _httpClient;

        public ServicoMeta(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MetaHttpClient");
        }

        public async Task<bool> EnviarSolitacaoAceiteContatoASync(Contato contato, string urlExtrato)
        {
            if (contato.Telefone == null || contato.Nome == null || contato.Nome == String.Empty)
            {
                return false;  
            } 
            WhatsAppMessageTemplate requestBody = new WhatsAppMessageTemplate(contato.Telefone,
                    "mob", "pt_BR");
            
            var bodyComponent = new Component("body");
            bodyComponent.parameters.Add(new Parameter("text", contato.Nome));
            requestBody.template.components.Add(bodyComponent);

            var buttonComponent = new Component("button");
            buttonComponent.sub_type = "url";
            buttonComponent.index = "0";
            buttonComponent.parameters.Add(new Parameter("text", urlExtrato));
            requestBody.template.components.Add(buttonComponent);
            var response = await _httpClient.PostAsJsonAsync<WhatsAppMessageTemplate>("", requestBody, JsonSerializerOptions.Default);
            var responseContent = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode;
        }
        
        public Task<bool> EnviarTesteASync(Contato contato)
        {
            throw new NotImplementedException();
        }
    }
}