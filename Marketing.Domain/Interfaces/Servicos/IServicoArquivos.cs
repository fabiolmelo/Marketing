using Marketing.Domain.Entidades;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoArquivos : IServico<ImportacaoEfetuada>
    {
        Task<string?> UploadArquivo(IFormFile arquivo);
        string GerarArquivoPdf(Estabelecimento estabelecimento, string arquivos, int posicao, String contentRootPath);
        List<DadosPlanilha> LerDados(string pathArquivo);

    }
}