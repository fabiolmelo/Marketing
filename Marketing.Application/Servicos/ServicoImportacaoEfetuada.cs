using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;

namespace Marketing.Application.Servicos
{
    public class ServicoImportacaoEfetuada : Servico<ImportacaoEfetuada>
    {
        public ServicoImportacaoEfetuada(IRepository<ImportacaoEfetuada> repository) : base(repository)
        {
        }
    }
}