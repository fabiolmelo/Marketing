using Marketing.Domain.Entidades;
using Microsoft.AspNetCore.Http;

namespace Marketing.Application.Validation
{
    public abstract class ValidationPlanilha
    {
        public abstract string Validate(List<DadosPlanilha> dadosPlanilhas);
    }
}