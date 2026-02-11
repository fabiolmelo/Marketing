namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioExportacao 
    {
        Task<List<ExportacaoV1>> BuscarRelatorioV1(string rede);
    }
}