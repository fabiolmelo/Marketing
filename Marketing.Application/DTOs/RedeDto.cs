using Microsoft.AspNetCore.Http;

namespace Marketing.Application.DTOs
{
    public class RedeDto
    {
        public string Nome { get; set; } = String.Empty;
        public DateTime DataCadastro { get; set; }
        public string? Logo { get; set; } 
        public IFormFile? ArquivoLogo { get; set; } 
    }
}