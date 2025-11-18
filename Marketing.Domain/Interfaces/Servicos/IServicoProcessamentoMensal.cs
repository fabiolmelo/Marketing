using Marketing.Domain.Entidades;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoProcessamentoMensal
    {
        Task GerarProcessamentoMensal(DateTime competencia, String webHostEnvironment,
                                    string caminhoApp, List<Contato> contatos);
    }
}