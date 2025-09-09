using System.Net.Http.Json;
using System.Text.Json;
using Marketing.Domain.Entidades;
using Marketing.Domain.Entidades.Meta;
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

        public async Task<ServicoExtratoResponseDto> EnviarExtrato(Contato contato,
                        Estabelecimento estabelecimento, string caminhoApp)
        {
            var urlExtrato = Path.Combine(caminhoApp, "Fechamento", "Download", estabelecimento.Cnpj);
            if (contato.Telefone == null || estabelecimento.RazaoSocial == null)
            {
                throw new Exception("Erro ao enviar extrato");
            } 
            WhatsAppMessageTemplate requestBody = new WhatsAppMessageTemplate(contato.Telefone,
                    "extrato", "pt_BR");
            
            var bodyComponent = new Component("body");
            bodyComponent.parameters.Add(new Parameter("text") { text = estabelecimento.RazaoSocial });
            requestBody.template.components.Add(bodyComponent);

            var buttonComponent = new Component("button");
            buttonComponent.sub_type = "url";
            buttonComponent.index = "0";
            buttonComponent.parameters.Add(new Parameter("text") { text = urlExtrato });
            requestBody.template.components.Add(buttonComponent);
            var response = await _httpClient.PostAsJsonAsync<WhatsAppMessageTemplate>("", requestBody, JsonSerializerOptions.Default);
            var responseContent = await response.Content.ReadAsStringAsync();
            return new ServicoExtratoResponseDto(response.StatusCode, responseContent);
        }

        public async Task<bool> EnviarSolitacaoAceiteContatoASync(Contato contato, string urlExtrato)
        {
            if (contato.Telefone == null || contato.Nome == null || contato.Nome == String.Empty)
            {
                return false;  
            } 
            WhatsAppMessageTemplate requestBody = new WhatsAppMessageTemplate(contato.Telefone,
                    "mob_img", "pt_BR");
            
            var headerComponent = new Component("header");
            headerComponent.parameters.Add(new Parameter("image") {image = new Image("https://i.postimg.cc/kgPt2cjs/img.png")});
            requestBody.template.components.Add(headerComponent);

            var bodyComponent = new Component("body");
            bodyComponent.parameters.Add(new Parameter("text") { text = contato.Nome });
            bodyComponent.parameters.Add(new Parameter("text") { text = contato.Nome });
            requestBody.template.components.Add(bodyComponent);

            var buttonComponent = new Component("button");
            buttonComponent.sub_type = "url";
            buttonComponent.index = "0";
            buttonComponent.parameters.Add(new Parameter("text") { text = urlExtrato });
            requestBody.template.components.Add(buttonComponent);
            var response = await _httpClient.PostAsJsonAsync<WhatsAppMessageTemplate>("", requestBody, JsonSerializerOptions.Default);
            var responseContent = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode;
        }
    }
}