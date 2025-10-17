using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Servicos;
using Marketing.Domain.PagedResponse;

namespace Marketing.Application.Servicos
{
    public class ServicoEstabelecimento : Servico<Estabelecimento>, IServicoEstabelecimento
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public ServicoEstabelecimento(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                                            }).ToList();

            foreach (var grupo in estabelecimentosPlanilha)
            {
                if (grupo.Cnpj != null)
                {
                    var estabelecimento = await _unitOfWork.repositorioEstabelecimento.GetByIdStringAsync(grupo.Cnpj);
                    var rede = await _unitOfWork.repositorioRede.GetByIdStringAsync(grupo.NomeRede);
                    if (estabelecimento == null && rede != null)
                    {
                        estabelecimento = new Estabelecimento()
                        {
                            Cnpj = grupo.Cnpj,
                            RazaoSocial = grupo.Restaurante ?? "",
                            Cidade = grupo.Cidade ?? "",
                            Uf = grupo.Uf ?? ""
                            //,Rede = rede
                            //,RedeNome = rede.Nome
                        };
                        await _unitOfWork.repositorioEstabelecimento.AddAsync(estabelecimento);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
        }
        
        // public async Task AtualizarAssociacaoEstabelecimentoContato(List<DadosPlanilha> dadosPlanilhas)
        // {
        //     foreach (DadosPlanilha linhaPlanilha in dadosPlanilhas)
        //     {
        //         if (linhaPlanilha.Fone != String.Empty && linhaPlanilha.Fone != null)
        //         {
        //             var estabelecimento = await _unitOfWork.repositorioEstabelecimento.FindEstabelecimentoIncludeContatoRede(linhaPlanilha.Cnpj);
        //             var contato = await _unitOfWork.repositorioContato.GetByIdStringAsync(linhaPlanilha.Fone);
        //             if (estabelecimento != null && contato != null)
        //             {

        //                 if (!estabelecimento.ContatoEstabelecimento.Any(x => x.ContatoTelefone == contato.Telefone))
        //                 {
        //                     estabelecimento.ContatoEstabelecimento..Add(contato);
        //                     _unitOfWork.repositorioEstabelecimento.Update(estabelecimento);
        //                     await _unitOfWork.CommitAsync();
        //                 }
        //             }    
        //         }
        //     }
        // }

        public async Task AtualizarAssociacaoEstabelecimentoRede(List<DadosPlanilha> dadosPlanilhas)
        {
            foreach (DadosPlanilha linhaPlanilha in dadosPlanilhas)
            {
                if (linhaPlanilha.Rede != String.Empty && linhaPlanilha.Rede != null)
                {
                    var estabelecimento = await FindEstabelecimentoIncludeContatoRede(linhaPlanilha.Cnpj);
                    var rede = await _unitOfWork.repositorioRede.GetByIdStringAsync(linhaPlanilha.Rede);

                    if (estabelecimento != null && rede != null)
                    {
                        if (estabelecimento.RedeNome != rede.Nome)
                        {
                            estabelecimento.Rede = rede;
                            estabelecimento.RedeNome = rede.Nome;
                            _unitOfWork.repositorioEstabelecimento.Update(estabelecimento);
                            await _unitOfWork.CommitAsync();
                        }
                    }
                }
            }
        }

        public async Task<PagedResponse<List<Estabelecimento>>> GetAllEstabelecimentos(int pageNumber, int pageSize, string? filtro)
        {
            return await _unitOfWork.repositorioEstabelecimento.GetAllEstabelecimentos(pageNumber, pageSize, filtro);
        }

        public async Task<Estabelecimento?> FindEstabelecimentoIncludeContatoRede(string cnpj)
        {
            return await _unitOfWork.repositorioEstabelecimento.FindEstabelecimentoIncludeContatoRede(cnpj);
        }

        public async Task AtualizarExtratosViaPlanilha(List<DadosPlanilha> dadosPlanilhas)
        {
            foreach (DadosPlanilha linha in dadosPlanilhas)
            {
                var estabelecimento = await _unitOfWork.repositorioEstabelecimento.FindEstabelecimentoIncludeContatoRede(linha.Cnpj);
                if (estabelecimento != null)
                {
                    var extrato = estabelecimento.ExtratoVendas.Any(x => x.Ano == linha.AnoMes.Year &&
                                                                     x.Mes == linha.AnoMes.Month &&
                                                                     x.EstabelecimentoCnpj == linha.Cnpj);
                    if (estabelecimento != null && extrato == false)
                    {
                        // estabelecimento.ExtratoVendas.Add(
                        //     new ExtratoVendas(
                        //         linha.AnoMes.Year,
                        //         linha.AnoMes.Month,
                        //         linha.AnoMes,
                        //         linha.TotalPedidos,
                        //         linha.PedidosComCocaCola,
                        //         linha.IncidenciaReal,
                        //         linha.Meta,
                        //         linha.PrecoUnitarioMedio,
                        //         linha.TotalPedidosNaoCapturados,
                        //         linha.ReceitaNaoCapturada,
                        //         linha.Cnpj)
                        //     );
                        // _unitOfWork.repositorioEstabelecimento.Update(estabelecimento);

                        var extratoNew = new ExtratoVendas(
                                                    linha.AnoMes.Year,
                                                    linha.AnoMes.Month,
                                                    linha.AnoMes,
                                                    linha.TotalPedidos,
                                                    linha.PedidosComCocaCola,
                                                    linha.IncidenciaReal,
                                                    linha.Meta,
                                                    linha.PrecoUnitarioMedio,
                                                    linha.TotalPedidosNaoCapturados,
                                                    linha.ReceitaNaoCapturada,
                                                    linha.Cnpj);
                        await _unitOfWork.GetRepository<ExtratoVendas>().AddAsync(extratoNew);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
        }
    }
}