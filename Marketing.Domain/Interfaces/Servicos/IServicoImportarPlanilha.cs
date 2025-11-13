using Marketing.Domain.Entidades;
using Microsoft.AspNetCore.Http;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoImportarPlanilha
    {
        Task<bool> ImportarPlanilha(IFormFile formFile, string Rede, string contentRootPath);
        Task<bool> ImportarPlanilhaNovo(IFormFile formFile, string rede, string contentRootPath);
        Task<bool> ImportarContato(IFormFile formFile, string contentRootPath);
        string ImportarPlanilhaOriginal(IFormFile formFile, TemplateImportarTipo template, string contentRootPath);
        
    }
}