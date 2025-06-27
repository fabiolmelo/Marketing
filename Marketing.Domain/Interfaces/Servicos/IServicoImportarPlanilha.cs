using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoImportarPlanilha
    {
        Task<bool> ImportarPlanilha(IFormFile formFile);
        
    }
}