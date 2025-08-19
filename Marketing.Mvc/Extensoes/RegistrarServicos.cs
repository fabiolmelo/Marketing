using Marketing.Application.Servicos;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;
using Marketing.Domain.Interfaces.UnitOfWork;
using Marketing.Infraestrutura.Repositorio;
using Marketing.Infraestrutura.Repositorio.UnitOfWork;

namespace Marketing.Mvc.Extensoes
{
    public static class RegistrarServicos
    {
        public static void AdicionarServicosAppIOC(this IServiceCollection servicos)
        {
            servicos.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            servicos.AddScoped(typeof(IServico<>), typeof(Servico<>));

            servicos.AddScoped<IServicoArquivos, ServicoArquivo>();
            servicos.AddScoped<IServicoContato, ServicoContato>();
            servicos.AddScoped<IServicoRede, ServicoRede>();
            servicos.AddScoped<IServicoGrafico, ServicoGrafico>();
            servicos.AddScoped<IServicoEstabelecimento, ServicoEstabelecimento>();
            servicos.AddScoped<IServicoExtratoVendas, ServicoExtratoVenda>();
            servicos.AddScoped<IServicoImportarPlanilha, ServicoImportarPlanilha>();
            servicos.AddScoped<IServicoProcessamentoMensal, ServicoProcessamentoMensal>();
            servicos.AddScoped<IServicoExtratoVendas, ServicoExtratoVenda>();
            servicos.AddScoped<IRepositorioEstabelecimento, RepositorioEstabelecimento>();
            servicos.AddScoped<IRepositorioRede, RepositorioRede>();
            servicos.AddScoped<IRepositorioProcessamentoMensal, RepositorioFechamentoMensal>();
            servicos.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureHttpClient(this IServiceCollection services,
                                               IConfiguration configuration)
        {
            var apiUrl = configuration["Meta:ApiUri"];
            var token =  configuration["Meta:Token"];
            if (apiUrl == null || token == null) throw new Exception("Configurations not found");

            services.AddHttpClient("MetaHttpClient", client =>
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            }).AddHttpMessageHandler(() => new BearerTokenHandler(token));
        }
    }
}