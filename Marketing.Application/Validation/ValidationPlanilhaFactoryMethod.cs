using Marketing.Domain.Entidades;

namespace Marketing.Application.Validation
{
    public abstract class ValidationPlanilhaFactoryMethod
    {
        public abstract ValidationPlanilha Criar(TemplateImportarTipo templateImportarTipo);
    }
}