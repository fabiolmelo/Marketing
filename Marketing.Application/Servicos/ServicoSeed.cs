using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Servicos;

namespace Marketing.Application.Servicos
{
    public class ServicoSeed : IServicoSeed
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicoSeed(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SeedConfiguracoesApp()
        {
            var configuracoes = await _unitOfWork.GetRepository<ConfiguracaoApp>().GetByIdAsync(1);
            if (configuracoes == null)
            {
                configuracoes = new ConfiguracaoApp()
                {
                    Id = 1,
                    MetaApiUrl = "https://graph.facebook.com/v22.0/718225858051731/messages",
                    MetaToken = "EAAkLO24ioZBMBPmfo9up0UUT6seDjPZCKJpt5XfqhkrWE4PA6I2NVgQqaHk1gQfIVtFwUlWXwg36ZCJqw6ISc1bc5hpsVCH3g2taN19etdmjE1wTNCT6gWYBpittMbeXBb30RIrvfvFnbd9CFg3BM3lppBgavZC6QVbvQHVy026mDjAaH3GKZAZBUlx5TCDNApU97eAeinplPj9eK4KGq1RKhvZCzZCZCfQiqu7mHWYSaSwZDZD",
                    FoneFrom = "15551498261",
                    LoteProcessamento = 16,
                    IntervaloEntreDisparos = 2,
                    AppUrl = "https://localhost:5252/"
                };
                await _unitOfWork.GetRepository<ConfiguracaoApp>().AddAsync(configuracoes);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task SeedRedes()
        {
            var existeRedes = await _unitOfWork.repositorioRede.GetAllAsync(1, 99999);
            if (existeRedes.Dados.Count == 0)
            {
                var redes = new List<Rede>();
                redes.Add(new Rede("DOMINOS"));
                redes.Add(new Rede("SPOLETO"));
                redes.Add(new Rede("KONI"));
                redes.Add(new Rede("CIB"));
                redes.Add(new Rede("GENDAI"));
                redes.Add(new Rede("LEBONTON"));
                redes.Add(new Rede("GURUME"));
                redes.Add(new Rede("BOBS"));
                await _unitOfWork.GetRepository<Rede>().AddRangeAsync(redes);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}