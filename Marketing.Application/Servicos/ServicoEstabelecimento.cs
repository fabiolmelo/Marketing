using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;
using Marketing.Domain.Interfaces.UnitOfWork;
using Marketing.Domain.PagedResponse;
using Microsoft.IdentityModel.Tokens;

namespace Marketing.Application.Servicos
{
    public class ServicoEstabelecimento : Servico<Estabelecimento>, IServicoEstabelecimento
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServicoRede _servicoRede;
        private readonly IServicoContato _servicoContato;
        private readonly IRepositorioEstabelecimento _repositorioEstabelecimento;
        public ServicoEstabelecimento(IUnitOfWork unitOfWork,
                                      IServicoRede servicoRede,
                                      IServicoContato servicoContato,
                                      IRepositorioEstabelecimento repositorioEstabelecimento) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _servicoRede = servicoRede;
            _servicoContato = servicoContato;
            _repositorioEstabelecimento = repositorioEstabelecimento;
        }

        public async Task AtualizarEstabelecimentoViaPlanilha(List<DadosPlanilha> dadosPlanilhas)
        {
            var estabelecimentosPlanilha = dadosPlanilhas.
                                            GroupBy(g => new { g.Cnpj, g.Restaurante, g.Cidade, g.Uf, g.Rede }).
                                            Select(x => new
                                            {
                                                Cnpj = x.Key.Cnpj,
                                                Restaurante = x.Key.Restaurante,
                                                Cidade = x.Key.Cidade,
                                                Uf = x.Key.Uf,
                                                NomeRede = x.Key.Rede 
                                            });

            foreach (var grupo in estabelecimentosPlanilha)
            {
                if (grupo.Cnpj != null)
                {
                    var estabelecimento = await _repositorioEstabelecimento.GetByIdStringAsync(grupo.Cnpj);
                    var rede = await _unitOfWork.GetRepository<Rede>().GetByIdStringAsync(grupo.NomeRede);
                    if (estabelecimento == null && rede != null)
                    {
                        estabelecimento = new Estabelecimento()
                        {
                            Cnpj = grupo.Cnpj,
                            RazaoSocial = grupo.Restaurante ?? "",
                            Cidade = grupo.Cidade ?? "",
                            Uf = grupo.Uf ?? "",
                            Rede = rede,
                            RedeNome = rede.Nome
                        };
                        await _unitOfWork.GetRepository<Estabelecimento>().AddAsync(estabelecimento);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
        }
        
        public async Task AtualizarAssociacaoEstabelecimentoContato(List<DadosPlanilha> dadosPlanilhas)
        {
            
            foreach (DadosPlanilha linhaPlanilha in dadosPlanilhas)
            {
                if (linhaPlanilha.Fone != String.Empty && linhaPlanilha.Fone != null)
                {
                    var estabelecimento = await _repositorioEstabelecimento.
                                            FindEstabelecimentoIncludeContatoRede(linhaPlanilha.Cnpj);

                    var contato = await _servicoContato.GetByIdStringAsync(linhaPlanilha.Fone);
                    if (estabelecimento != null && contato != null)
                    {
                        if (!estabelecimento.Contatos.Contains(contato))
                        {
                            estabelecimento.Contatos.Add(contato);
                            _unitOfWork.GetRepository<Estabelecimento>().Update(estabelecimento);
                            await _unitOfWork.CommitAsync();
                        }
                    }    
                }
            }
        }

        public async Task AtualizarAssociacaoEstabelecimentoRede(List<DadosPlanilha> dadosPlanilhas)
        {
            foreach (DadosPlanilha linhaPlanilha in dadosPlanilhas)
            {
                if (linhaPlanilha.Rede != String.Empty && linhaPlanilha.Rede != null)
                {
                    var estabelecimento = await _repositorioEstabelecimento.
                                FindEstabelecimentoIncludeContatoRede(linhaPlanilha.Cnpj);
                    var rede = await _servicoRede.GetByIdStringAsync(linhaPlanilha.Rede);

                    if (estabelecimento != null && rede != null)
                    {
                        estabelecimento.Rede = rede;
                        estabelecimento.RedeNome = rede.Nome;
                        _unitOfWork.GetRepository<Estabelecimento>().Update(estabelecimento);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
        }

        public async Task<PagedResponse<Estabelecimento>> GetAllEstabelecimentos(int pageNumber, int pageSize, string? filtro)
        {
            return await _repositorioEstabelecimento.GetAllEstabelecimentos(pageNumber, pageSize, filtro);
        }
    }
}