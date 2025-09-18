using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;
using Marketing.Domain.PagedResponse;

namespace Marketing.Application.Servicos
{
    public class ServicoEstabelecimento : Servico<Estabelecimento>, IServicoEstabelecimento
    {
        private readonly IRepositorioEstabelecimento _repositoryEstabelecimento;
        private readonly IRepositorioRede _repositoryRede;
        private readonly IRepositorioContato _repositorioContato;
        public ServicoEstabelecimento(IRepositorioEstabelecimento repositoryEstabelecimento,
                                      IRepositorioRede repositoryRede,
                                      IRepositorioContato repositorioContato) : base(repositoryEstabelecimento)
        {
            _repositoryEstabelecimento = repositoryEstabelecimento;
            _repositoryRede = repositoryRede;
            _repositorioContato = repositorioContato;
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
                    var estabelecimento = await _repositoryEstabelecimento.GetByIdStringAsync(grupo.Cnpj);
                    var rede = await _repositoryRede.GetByIdStringAsync(grupo.NomeRede);
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
                        await _repositoryEstabelecimento.AddAsync(estabelecimento);
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
                    var estabelecimento = await _repositoryEstabelecimento.FindEstabelecimentoIncludeContatoRede(linhaPlanilha.Cnpj);
                    var contato = await _repositorioContato.GetByIdStringAsync(linhaPlanilha.Fone);
                    if (estabelecimento != null && contato != null)
                    {
                        if (!estabelecimento.Contatos.Contains(contato))
                        {
                            estabelecimento.Contatos.Add(contato);
                            _repositoryEstabelecimento.Update(estabelecimento);
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
                    var estabelecimento = await FindEstabelecimentoIncludeContatoRede(linhaPlanilha.Cnpj);
                    var rede = await _repositoryRede.GetByIdStringAsync(linhaPlanilha.Rede);

                    if (estabelecimento != null && rede != null)
                    {
                        estabelecimento.Rede = rede;
                        estabelecimento.RedeNome = rede.Nome;
                        _repositoryEstabelecimento.Update(estabelecimento);
                    }
                }
            }
        }

        public async Task<PagedResponse<Estabelecimento>> GetAllEstabelecimentos(int pageNumber, int pageSize, string? filtro)
        {
            return await _repositoryEstabelecimento.GetAllEstabelecimentos(pageNumber, pageSize, filtro);
        }

        public async Task<Estabelecimento?> FindEstabelecimentoIncludeContatoRede(string cnpj)
        {
            return await _repositoryEstabelecimento.FindEstabelecimentoIncludeContatoRede(cnpj);
        }
    }
}