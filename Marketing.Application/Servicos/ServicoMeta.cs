using System.Net.Http.Json;
using System.Text.Json;
using Marketing.Domain.Entidades;
using Marketing.Domain.Entidades.Meta;
using Marketing.Domain.Interfaces.IHttpClient;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Servicos;


namespace Marketing.Application.Servicos
{
    public class ServicoMeta : IServicoMeta
    {
        private readonly HttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;

        public ServicoMeta(IHttpClientsFactoryPerson httpClientFactory, IUnitOfWork unitOfWork)
        {
            _httpClient = httpClientFactory.Client("MetaHttpClient");
            _unitOfWork = unitOfWork;
        }

        public async Task<ServicoExtratoResponseDto> EnviarExtrato(Contato contato,
                        Estabelecimento estabelecimento, string caminhoApp)
        {
            var urlExtrato = Path.Combine("Fechamento", "Download", estabelecimento.Cnpj, contato.Telefone);
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

        public async Task<ServicoExtratoResponseDto> EnviarExtratoV2(string idMensagem)
        {
            var urlExtrato = $"{idMensagem}";
            var envio = await _unitOfWork.repositorioEnvioMensagemMensal.GetByIdStringAsync(idMensagem);
            if (envio == null) throw new Exception("Erro enviando status!");
            var contato = await _unitOfWork.repositorioContato.GetByIdStringAsync(envio.ContatoTelefone);
            var estabelecimento = await _unitOfWork.repositorioEstabelecimento.GetByIdStringAsync(envio.EstabelecimentoCnpj);
            if (contato == null || estabelecimento == null) throw new Exception("Erro enviando status!");
            WhatsAppMessageTemplate requestBody = new WhatsAppMessageTemplate(contato.Telefone, "extrato_V2", "pt_BR");
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