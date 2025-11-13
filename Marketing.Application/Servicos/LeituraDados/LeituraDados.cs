using Marketing.Domain.DTOs;

namespace Marketing.Application.Servicos.LeituraDados
{
    public abstract class LeituraDados
    {
        public abstract ResponseValidation LerDados(string pathArquivo);
    }
}