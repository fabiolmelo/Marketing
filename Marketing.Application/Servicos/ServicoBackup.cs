using Marketing.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Marketing.Application.Servicos
{
    public class ServicoBackup : BackgroundService
    {
        private DataContext? _localContext;
        private DataContextMySql? _cloudContext;
        public IServiceProvider Services { get; }


        public ServicoBackup(IServiceProvider services)
        {
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = Services.CreateScope())
                {
                    _localContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                    _cloudContext = scope.ServiceProvider.GetRequiredService<DataContextMySql>();
                    await SyncLocalWithCloud();
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task SyncLocalWithCloud()
        {
            // if (_cloudContext == null || _localContext == null) return;
            // // var contatoNuvem = await _cloudContext.Contatos.ToListAsync();
            // // var contatoLocal = await _localContext.Contatos.ToListAsync();
            // // var contatoBackup = contatoLocal.Except(contatoNuvem).ToList();
            // // await _cloudContext.Contatos.AddRangeAsync(contatoBackup);

            // var estabelecimentoNuvem = await _cloudContext.Estabelecimentos.AsNoTracking().ToListAsync();
            // var estabelecimentoLocal = await _localContext.Estabelecimentos.AsNoTracking().ToListAsync();
            // var estabelecimentoBackup = estabelecimentoLocal.Except(estabelecimentoNuvem).Distinct().ToList();

            // foreach (var estabelecimento in estabelecimentoBackup)
            // {
            //     var existe = _cloudContext.Estabelecimentos.AsNoTracking().Any(x=>x.Cnpj == estabelecimento.Cnpj);
            //     if (!existe)
            //     {
            //         await _cloudContext.Estabelecimentos.AddAsync(estabelecimento);
            //         await _cloudContext.SaveChangesAsync();
            //         _cloudContext.ChangeTracker.Clear();
            //     }
            // }
           
        }
    }
}