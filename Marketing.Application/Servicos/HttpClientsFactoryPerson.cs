using System.Net.Http.Headers;
using Marketing.Domain.Interfaces.IHttpClient;
using Microsoft.Extensions.Configuration;

namespace Marketing.Application.Servicos
{
    public class HttpClientsFactoryPerson : IHttpClientsFactoryPerson
    {
        public static Dictionary<string, HttpClient> HttpClients { get; set; } = new Dictionary<string, HttpClient>();
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public HttpClientsFactoryPerson(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            InicializarHttpClientEstatico();
        }

        // private void Initialize()
        // {
        //     HttpClient client = _httpClientFactory.CreateClient("MetaHttpClient");;
        //     HttpClients.Add("MetaHttpClient", client);
        // }

        private void InicializarHttpClientEstatico()
        {
            var apiUrl = _configuration["Meta:ApiUrl"];
            var token = _configuration["Meta:Token"];
            if (apiUrl == null || token == null) throw new Exception("Configurations not found");
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(apiUrl)
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpClients.Add("MetaHttpClient", client);
        }
        
        public HttpClient Client(string key)
        {
            return Clients()[key];
        }

        public Dictionary<string, HttpClient> Clients()
        {
            return HttpClients;
        }
    }
}