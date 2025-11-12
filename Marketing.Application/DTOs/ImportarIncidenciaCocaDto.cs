using Microsoft.AspNetCore.Http;

namespace Marketing.Application.DTOs
{
    public class ImportarIncidenciaCocaDto
    {
        public int Template { get; set; }
        public IFormFile? arquivoEnviado { get; set; }
    }
}