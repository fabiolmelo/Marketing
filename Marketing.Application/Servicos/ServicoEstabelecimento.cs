using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Repositorio;
using Marketing.Domain.Interfaces.Servicos;
using Marketing.Domain.Interfaces.UnitOfWork;
using Marketing.Domain.PagedResponse;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Immutable;

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
            var estabelecimentosPlanilha = dadosPlanilhas.Where(x=>!x.Cnpj.IsNullOrEmpty()).
                                            GroupBy(g => new { g.Cnpj, g.Restaurante, g.Cidade, g.Uf }).
                                            Select(x => new
                                            {
                                                Cnpj = x.Key.Cnpj,
                                                Restaurante = x.Key.Restaurante,
                                                Cidade = x.Key.Cidade,
                                                Uf = x.Key.Uf
                                            });

            foreach(var grupo in estabelecimentosPlanilha)
            {
                if (grupo.Cnpj != null)
                {
                    var estabelecimento = await _repositorioEstabelecimento.GetByIdStringAsync(grupo.Cnpj);
                    if (estabelecimento == null)
                    {
                        estabelecimento = new Estabelecimento()
                        {
                            Cnpj = grupo.Cnpj,
                            RazaoSocial = grupo.Restaurante ?? "",
                            Cidade = grupo.Cidade ?? "",
                            Uf = grupo.Uf ?? ""
                        };
                        await _unitOfWork.GetRepository<Estabelecimento>().AddAsync(estabelecimento);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
        }
        
        public async Task AtualizarAssociacaoEstabelecimentoContato(List<DadosPlanilha> dadosPlanilhas)
        {
            var estabelecimentos = dadosPlanilhas.Where(x => !x.Fone.IsNullOrEmpty()).ToList();

            foreach (DadosPlanilha linha in estabelecimentos)
            {
                if (linha.Cnpj != null && linha.Fone != null)
                {
                    var estabelecimento = await _repositorioEstabelecimento.
                                            FindEstabelecimentoIncludeContatoRede(linha.Cnpj);
                    var contato = await _servicoContato.GetByIdStringAsync(linha.Fone);

                    if (estabelecimento == null || contato == null)
                    {
                        throw new Exception("Erro fatal");
                    }
                    if (!estabelecimento.Contatos.Contains(contato))
                    {
                        estabelecimento.Contatos.Add(contato);
                        _unitOfWork.GetRepository<Estabelecimento>().
                                Update(estabelecimento);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
        }

        public async Task AtualizarAssociacaoEstabelecimentoRede(List<DadosPlanilha> dadosPlanilhas)
        {
            var estabelecimentos = dadosPlanilhas.Where(x => !x.Rede.IsNullOrEmpty()).ToList();
            foreach (DadosPlanilha linha in estabelecimentos)
            {
                if (linha.Cnpj != null  && linha.Rede != null)
                {
                    var estabelecimento = await _repositorioEstabelecimento.
                                            FindEstabelecimentoIncludeContatoRede(linha.Cnpj);
                    var rede = await _servicoRede.FindByPredicate(x => x.Nome == linha.Rede);

                    if (estabelecimento == null || rede == null)
                    {
                        throw new Exception("Erro fatal");
                    }
                    if (estabelecimento.RedeId == null)
                    {
                        estabelecimento.Rede = rede;
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