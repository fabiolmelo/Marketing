using Marketing.Domain.Entidades;
using Microsoft.AspNetCore.Http;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoArquivos : IServico<ImportacaoEfetuada>
    {
        Task<string?> UploadArquivo(IFormFile arquivo, string contentRootPath);
        string GerarArquivoPdf(Estabelecimento estabelecimento, string arquivos, int posicao,
                               String contentRootPath, string caminhoApp);
        List<DadosPlanilha> LerDados(string pathArquivo, string rede);
        Task<bool> AtualizarContatoViaPlanilhaEmailMarketing(string pathArquivo);
        void GerarRelatorioEnvios(string pathArquivo, List<Mensagem> mensagens,
                                         List<ResumoMensagem> resumo, string pathArquivoBase);
    }
}