using System.Text.Json;
using Marketing.Domain.Entidades.ReceitaWS;
using Marketing.Domain.Interfaces.IHttpClient;
using Marketing.Domain.Interfaces.Servicos;

namespace Marketing.Application.Servicos
{
    public class ServicoReceitaFederal : IServicoReceitaFederal
    {
        private readonly HttpClient _httpClient;

        public ServicoReceitaFederal(IHttpClientsFactoryPerson httpClientFactory)
        {
            _httpClient = httpClientFactory.Client("ReceitaWS");
        }

        public async Task<ClienteReceitaFederal?> ConsultarDadosReceitaFederal(string cnpj)
        {
            try
            {
                var response = await _httpClient.GetAsync(cnpj);
                if (!response.IsSuccessStatusCode) return null;
                var cliente = await JsonSerializer.DeserializeAsync<ClienteReceitaFederal>(response.Content.ReadAsStream());
                return cliente;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}