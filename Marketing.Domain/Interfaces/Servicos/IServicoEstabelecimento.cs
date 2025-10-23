using Marketing.Domain.Entidades;
using Marketing.Domain.PagedResponse;

namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoEstabelecimento : IServico<Estabelecimento>
    {
        Task<PagedResponse<List<Estabelecimento>>> GetAllEstabelecimentos(int? pageNumber, int? pageSize, string? filtro);
        Task AtualizarEstabelecimentoViaPlanilha(List<DadosPlanilha> dadosPlanilhas);
        //Task AtualizarAssociacaoEstabelecimentoContato(List<DadosPlanilha> dadosPlanilhas);
        Task AtualizarAssociacaoEstabelecimentoRede(List<DadosPlanilha> dadosPlanilhas);
        Task<Estabelecimento?> FindEstabelecimentoIncludeContatoRede(string cnpj);
        Task AtualizarExtratosViaPlanilha(List<DadosPlanilha> dadosPlanilhas);
        Task<bool> AtualizarDadosCadastraisViaReceitaFederal(string cnpj, bool? forcarUpdate = false);
    }
}