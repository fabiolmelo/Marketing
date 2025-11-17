using Marketing.Domain.Entidades;

namespace Marketing.Application.Validation
{
    public class ValidationPlanilhaFactory : ValidationPlanilhaFactoryMethod
    {
        public override ValidationPlanilha Criar(TemplateImportarTipo templateImportarTipo)
        {
            switch (templateImportarTipo)
            {
                case TemplateImportarTipo.DOMINUS:
                    return new ValidationPlanilhaDominos();
                case TemplateImportarTipo.GRUPO_TRIGO:
                    return new ValidationPlanilhaGrupoTrigo();
                default:
                    throw new Exception("Template indefinido!");
            }
        }
    }
}