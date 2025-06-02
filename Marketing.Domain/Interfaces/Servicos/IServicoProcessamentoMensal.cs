namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoProcessamentoMensal
    {
        Task GerarProcessamentoMensal(DateTime competencia, String webHostEnvironment);
    }
}