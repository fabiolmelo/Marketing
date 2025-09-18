using Marketing.Domain.Entidades;
using Microsoft.AspNetCore.Http;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoArquivos : IServico<ImportacaoEfetuada>
    {
        Task<string?> UploadArquivo(IFormFile arquivo);
        string GerarArquivoPdf(Estabelecimento estabelecimento, string arquivos, int posicao,
                               String contentRootPath, string caminhoApp);
        List<DadosPlanilha> LerDados(string pathArquivo);

    }
}