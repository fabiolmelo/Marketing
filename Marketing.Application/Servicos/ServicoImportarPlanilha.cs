using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.IUnitOfWork;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Http;

namespace Marketing.Application.Servicos
{
    public class ServicoImportarPlanilha : IServicoImportarPlanilha
    {
        private readonly IServicoContato _servicoContato;
        private readonly IServicoRede _servicoRede;
        private readonly IServicoArquivos _servicoArquivos;
        private readonly IServicoEstabelecimento _servicoEstabelecimento;
        private readonly IServicoExtratoVendas _servicoExtratoVendas;
        private readonly IUnitOfWork _unitOfWork;

        public ServicoImportarPlanilha(IServicoContato servicoContato,
                                       IServicoRede servicoRede,
                                       IServicoArquivos servicoArquivos,
                                       IServicoEstabelecimento servicoEstabelecimento,
                                       IServicoExtratoVendas servicoExtratoVendas,
                                       IUnitOfWork unitOfWork)
        {
            _servicoContato = servicoContato;
            _servicoRede = servicoRede;
            _servicoArquivos = servicoArquivos;
            _servicoEstabelecimento = servicoEstabelecimento;
            _servicoExtratoVendas = servicoExtratoVendas;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ImportarContato(IFormFile formFile, string contentRootPath)
        {
            var arquivoImportado = await _servicoArquivos.UploadArquivo(formFile, contentRootPath);
            if (arquivoImportado == null)
                return false;
            return await _servicoArquivos.AtualizarContatoViaPlanilhaEmailMarketing(arquivoImportado);
        }

        public async Task<bool> ImportarPlanilha(IFormFile formFile, string rede, string contentRootPath)
        {
            var arquivoImportado = await _servicoArquivos.UploadArquivo(formFile, contentRootPath);
            if (arquivoImportado == null)
                return false;
            var dadosPlanilha = _servicoArquivos.LerDados(arquivoImportado, rede);
            var importacaoEfetuada = new ImportacaoEfetuada(arquivoImportado);
            importacaoEfetuada.AdicionarDadosPlanilha(dadosPlanilha);
            await _servicoArquivos.AddAsync(importacaoEfetuada);
            await _servicoArquivos.CommitAsync();
            await _servicoRede.AtualizarRedesViaPlanilha(dadosPlanilha);
            await _servicoContato.AtualizarContatosViaPlanilha(dadosPlanilha);
            await _servicoEstabelecimento.AtualizarEstabelecimentoViaPlanilha(dadosPlanilha);
            //await _servicoEstabelecimento.AtualizarAssociacaoEstabelecimentoContato(dadosPlanilha);
            await _servicoContato.AtualizarAssociacaoContatoEstabelecimento(dadosPlanilha);
            await _servicoEstabelecimento.AtualizarAssociacaoEstabelecimentoRede(dadosPlanilha);
            await _servicoEstabelecimento.AtualizarExtratosViaPlanilha(dadosPlanilha);
            return true;
        }

        public async Task<bool> ImportarPlanilhaNovo(IFormFile formFile, string rede, string contentRootPath)
        {
            var arquivoImportado = await _servicoArquivos.UploadArquivo(formFile, contentRootPath);
            if (arquivoImportado == null)
                return false;
            var dadosPlanilha = _servicoArquivos.LerDados(arquivoImportado, rede);
            var importacaoEfetuada = new ImportacaoEfetuada(arquivoImportado);
            importacaoEfetuada.AdicionarDadosPlanilha(dadosPlanilha);
            await _servicoArquivos.AddAsync(importacaoEfetuada);
            await SalvarImportacaoPlanilha(dadosPlanilha);
            if (File.Exists(arquivoImportado)) File.Delete(arquivoImportado);
            return true;
        }
        
        public async Task<bool> SalvarImportacaoPlanilha(List<DadosPlanilha> dadosPlanilha)
        {       
            await _servicoArquivos.CommitAsync();
            await AtualizarContatosViaPlanilha(dadosPlanilha);
            await AtualizarEstabelecimentoViaPlanilha(dadosPlanilha);
            await AtualizarAssociacaoContatoEstabelecimento(dadosPlanilha);
            await AtualizarExtratosViaPlanilha(dadosPlanilha);
            return true;
        }

        private async Task AtualizarContatosViaPlanilha(List<DadosPlanilha> dadosPlanilha)
        {
            var contatosCadastrados = await _unitOfWork.repositorioContato.GetAll();
            var foneLista = dadosPlanilha.Where(x=> x.Fone != String.Empty)
                                                  .Select(x => x.Fone).Distinct();
            foreach (var telefone in foneLista)
            {
                if (!contatosCadastrados.Any(x=>x.Telefone == telefone))
                {
                    var contato = new Contato(telefone);
                    contato.OrigemContato = OrigemContato.PlanilhaIncidencia;
                    contato.DataCadastro = DateTime.UtcNow;
                    await _unitOfWork.repositorioContato.AddAsync(contato);
                    await _unitOfWork.CommitAsync();
                }
            }
        }
        private async Task AtualizarEstabelecimentoViaPlanilha(List<DadosPlanilha> dadosPlanilha)
        {
            var estabelecimentosCadastrados = await _unitOfWork.repositorioEstabelecimento.GetAllEstabelecimentos(1, 999999, "");
            foreach (var estabelecimentoPlanilha in dadosPlanilha.Select(x => new { x.Cnpj, x.Restaurante, x.Cidade, x.Uf, x.Rede }).Distinct())
            {
                var estabelecimento = new Estabelecimento()
                {
                    Cnpj = estabelecimentoPlanilha.Cnpj,
                    RazaoSocial = estabelecimentoPlanilha.Restaurante,
                    Cidade = estabelecimentoPlanilha.Cidade,
                    Uf = estabelecimentoPlanilha.Uf,
                    RedeNome = estabelecimentoPlanilha.Rede
                };
                if (!estabelecimentosCadastrados.Dados.Any(x => x.Cnpj == estabelecimentoPlanilha.Cnpj))
                {
                    await _unitOfWork.repositorioEstabelecimento.AddAsync(estabelecimento);
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    _unitOfWork.repositorioEstabelecimento.Update(estabelecimento);
                    await _unitOfWork.CommitAsync();
                }
            }
        }
        private async Task AtualizarAssociacaoContatoEstabelecimento(List<DadosPlanilha> dadosPlanilha)
        {
            var contatoEstabelecimentoCadastrado = await _unitOfWork.GetRepository<ContatoEstabelecimento>().GetAll();
            foreach (var contatoEstabelecimentoPlanilha in dadosPlanilha
                                                        .Where(x=>x.Fone != String.Empty)
                                                        .Select(x => new { x.Cnpj, x.Fone }).Distinct())
            {
                if (!contatoEstabelecimentoCadastrado.Any(x => x.EstabelecimentoCnpj == contatoEstabelecimentoPlanilha.Cnpj &&
                                                     x.ContatoTelefone == contatoEstabelecimentoPlanilha.Fone))
                {
                    var contatoEstabelecimento = new ContatoEstabelecimento()
                    {
                        EstabelecimentoCnpj = contatoEstabelecimentoPlanilha.Cnpj,
                        ContatoTelefone = contatoEstabelecimentoPlanilha.Fone
                    };
                    await _unitOfWork.GetRepository<ContatoEstabelecimento>().AddAsync(contatoEstabelecimento);
                    await _unitOfWork.CommitAsync();
                }
            }
        }
        private async Task AtualizarExtratosViaPlanilha(List<DadosPlanilha> dadosPlanilhas)
        {
            foreach(var dadosPlanilha in dadosPlanilhas)
            {
                var cadastrado = await _unitOfWork.GetRepository<ExtratoVendas>().FindByPredicate(x => x.EstabelecimentoCnpj == dadosPlanilha.Cnpj &&
                                                                                                 x.Competencia == dadosPlanilha.AnoMes);
                var extratoVendas = new ExtratoVendas(
                                    dadosPlanilha.AnoMes.Year,
                                    dadosPlanilha.AnoMes.Month,
                                    dadosPlanilha.AnoMes,
                                    dadosPlanilha.TotalPedidos,
                                    dadosPlanilha.PedidosComCocaCola,
                                    dadosPlanilha.IncidenciaReal,
                                    dadosPlanilha.Meta,
                                    dadosPlanilha.PrecoUnitarioMedio,
                                    dadosPlanilha.TotalPedidosNaoCapturados,
                                    dadosPlanilha.ReceitaNaoCapturada,
                                    dadosPlanilha.Cnpj
                                );                                                               
                if(cadastrado == null)
                {                   
                    await _unitOfWork.GetRepository<ExtratoVendas>().AddAsync(extratoVendas);
                }
                else
                {
                    _unitOfWork.GetRepository<ExtratoVendas>().Update(extratoVendas);           
                }
                await _unitOfWork.CommitAsync();
            }
        }
        public string ImportarPlanilhaOriginal(IFormFile formFile, TemplateImportarTipo template, string contentRootPath)
        {
            throw new NotImplementedException();
        }
    }
}