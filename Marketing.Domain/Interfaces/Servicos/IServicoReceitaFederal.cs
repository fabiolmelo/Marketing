using Marketing.Domain.Entidades.ReceitaWS;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoReceitaFederal
    {
        Task<ClienteReceitaFederal?> ConsultarDadosReceitaFederal(string cnpj);
    }
}