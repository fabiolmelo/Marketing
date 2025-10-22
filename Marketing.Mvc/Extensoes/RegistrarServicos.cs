using System.Net.Http.Headers;
using Marketing.Application.Servicos;
using Marketing.Domain.Interfaces.IHttpClient;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;
using Marketing.Infraestrutura.Repositorio;
using Marketing.Infraestrutura.Repositorio.UnitOfWork;

namespace Marketing.Mvc.Extensoes
{
    public static class RegistrarServicos
    {
        public static void AdicionarServicosAppIOC(this IServiceCollection servicos)
        {
            servicos.AddScoped<IUnitOfWork, UnitOfWork>();
            servicos.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            servicos.AddScoped(typeof(IServico<>), typeof(Servico<>));

            servicos.AddScoped<IServicoArquivos, ServicoArquivo>();
            servicos.AddScoped<IServicoContato, ServicoContato>();
            servicos.AddScoped<IServicoRede, ServicoRede>();
            servicos.AddScoped<IServicoGrafico, ServicoGrafico>();
            servicos.AddScoped<IServicoGraficoRevisado, ServicoGraficoRevisado>();
            servicos.AddScoped<IServicoMeta, ServicoMeta>();
            servicos.AddScoped<IServicoEstabelecimento, ServicoEstabelecimento>();
            servicos.AddScoped<IServicoExtratoVendas, ServicoExtratoVenda>();
            servicos.AddScoped<IServicoImportarPlanilha, ServicoImportarPlanilha>();
            servicos.AddScoped<IServicoProcessamentoMensal, ServicoProcessamentoMensal>();
            servicos.AddScoped<IServicoExtratoVendas, ServicoExtratoVenda>();
            servicos.AddScoped<IServicoEnvioMensagemMensal, ServicoEnvioMensagemMensal>();
            servicos.AddScoped<IServicoSeed, ServicoSeed>();

            servicos.AddScoped<IRepositorioContato, RepositorioContato>();
            servicos.AddScoped<IRepositorioExtratoVendas, RepositorioExtratoVendas>();
            servicos.AddScoped<IRepositorioEstabelecimento, RepositorioEstabelecimento>();
            servicos.AddScoped<IRepositorioProcessamentoMensal, RepositorioFechamentoMensal>();
            servicos.AddScoped<IRepositorioEnvioMensagemMensal, RepositorioEnvioMensagemMensal>();
            servicos.AddScoped<IRepositorioRede, RepositorioRede>();
            servicos.AddSingleton<IHttpClientsFactoryPerson, HttpClientsFactoryPerson>();
        }

        public static void ConfigureHttpClient(IServiceCollection services,
                                               IConfiguration configuration)
        {
            var apiUrl = configuration["Meta:ApiUrl"];
            var token = configuration["Meta:Token"];
            if (apiUrl == null || token == null) throw new Exception("Configurations not found");

            services.AddHttpClient("MetaHttpClient", client =>
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            })
            .AddHttpMessageHandler(x => new BearerTokenHandler(token))
            .SetHandlerLifetime(TimeSpan.FromMinutes(10));

            services.AddHttpClient("ReceitaWS", client =>
            {
                client.BaseAddress = new Uri("https://receitaws.com.br/v1/cnpj/");
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(10));
        }
    }
}