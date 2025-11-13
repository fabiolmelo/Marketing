using Marketing.Domain.DTOs;
using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Http;

namespace Marketing.Application.Validation
{
    public class ValidationPlanilhaDominos : ValidationPlanilha
    {
        private ResponseValidation responseValidation = new ResponseValidation();

        public override string Validate(List<DadosPlanilha> dadosPlanilhas)
        {
            ValidateDuplicate();
            return responseValidation.ToString();
        }

        private void ValidateDuplicate()
        {
            
        }
    }
}