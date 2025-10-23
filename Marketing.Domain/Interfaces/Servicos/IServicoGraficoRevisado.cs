using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoGraficoRevisado
    {
        void GerarGrafico(Estabelecimento estabelecimento, string contentRootPath);
        public Task<string> GerarArquivoPdf(Estabelecimento estabelecimento, string arquivoPdf,
                                   int posicao, String contentRootPath,
                                   string caminhoApp);
    }
}