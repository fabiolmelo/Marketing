using Marketing.Domain.Entidades;

namespace Marketing.Application.Servicos.LeituraDados
{
    public class LeituraDadosFactory : LeituraDadosFactoryMethod
    {
        public override LeituraDados Criar(TemplateImportarTipo templateImportarTipo)
        {
            switch (templateImportarTipo)
            {
                case TemplateImportarTipo.DOMINUS:
                    return new LeituraDadosDominos();
                case TemplateImportarTipo.GRUPO_TRIGO:
                    return new LeituraDadosGrupoTrigo();
                default:
                    throw new Exception("Template indefinido!");
            }
        }
    }
}