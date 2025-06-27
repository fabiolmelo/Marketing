using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoGrafico
    {
        void GerarGrafico(Estabelecimento estabelecimento, string contentRootPath);
    }
}