using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;

namespace Marketing.Application.Servicos
{
    public class ServicoExtratoVenda : Servico<ExtratoVendas>, IServicoExtratoVendas
    {
        
        private readonly IRepositorioExtratoVendas _repositorioExtratoVendas;

        public ServicoExtratoVenda(IRepositorioExtratoVendas repositorioExtratoVendas) : base(repositorioExtratoVendas)
        {
            _repositorioExtratoVendas = repositorioExtratoVendas;
        }

        public async Task<DateTime> BuscarCompetenciaVigente()
        {
            return await _repositorioExtratoVendas.BuscarCompetenciaVigente();
        }
    }
}