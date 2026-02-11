using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoExport
    {
        Byte[] ExportarFechamentoV1(List<FechamentoV1> fechamentos);
        Task<FechamentoV1> GetFechamentoV1PorRede(string rede);
    }
}