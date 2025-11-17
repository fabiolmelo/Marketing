using Marketing.Domain.DTOs;
using Marketing.Domain.Entidades;

namespace Marketing.Application.Validation
{
    public abstract class ValidationPlanilha
    {
        public abstract ResponseValidation Validate(List<DadosPlanilha> dadosPlanilhas);
    }
}