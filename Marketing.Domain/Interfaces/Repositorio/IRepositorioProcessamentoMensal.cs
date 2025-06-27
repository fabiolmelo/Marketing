using Marketing.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marketing.Domain.Interfaces.Repositorio
{
    public interface IRepositorioProcessamentoMensal : IRepository<FechamentoMensal>
    {
        Task<List<Estabelecimento>> GetAllEstabelecimentosParaGerarPdf(DateTime competencia); 
    }
}
