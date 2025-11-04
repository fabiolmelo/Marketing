using Microsoft.AspNetCore.Http;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoImportarPlanilha
    {
        Task<bool> ImportarPlanilha(IFormFile formFile, string Rede);
        Task<bool> ImportarPlanilhaNovo(IFormFile formFile, string rede);
        Task<bool> ImportarContato(IFormFile formFile);
    }
}