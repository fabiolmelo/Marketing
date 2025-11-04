using Microsoft.AspNetCore.Http;

namespace Marketing.Application.DTOs
{
    public class ImportarIncidenciaDto
    {
        public string? Rede { get; set; }
        public IFormFile? arquivoEnviado { get; set; }
    }
}