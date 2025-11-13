using Marketing.Domain.Entidades;

namespace Marketing.Application.Servicos.LeituraDados
{
    public abstract class LeituraDadosFactoryMethod
    {
        public abstract LeituraDados Criar(TemplateImportarTipo templateImportarTipo);
    }
}