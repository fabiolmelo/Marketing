using Microsoft.AspNetCore.Http;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoImportarPlanilha
    {
        Task<bool> ImportarPlanilha(IFormFile formFile);
        
    }
}