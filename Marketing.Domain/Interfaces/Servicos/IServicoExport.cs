using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoExport
    {
        Task<List<FechamentoV1>> GerarFechamentoV1(string pathArquivoBase);
        Byte[] ExportarFechamentoV1(List<FechamentoV1> fechamentos);
        Task<List<ExportacaoV1>> GetFechamentoV1PorRede(string rede);

    }
}