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

        public async Task<bool> ImportarContato(IFormFile formFile)
        {
            var arquivoImportado = await _servicoArquivos.UploadArquivo(formFile);
            if (arquivoImportado == null)
                return false;
            await _servicoArquivos.AtualizarContatoViaPlanilhaEmailMarketing(arquivoImportado);
            return true;
        }

        public async Task<bool> ImportarPlanilha(IFormFile formFile, string rede)
        {
            var arquivoImportado = await _servicoArquivos.UploadArquivo(formFile);
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

        public async Task<bool> ImportarPlanilhaNovo(IFormFile formFile, string rede)
        {
            var arquivoImportado = await _servicoArquivos.UploadArquivo(formFile);
            if (arquivoImportado == null)
                return false;
            var dadosPlanilha = _servicoArquivos.LerDados(arquivoImportado, rede);
            var importacaoEfetuada = new ImportacaoEfetuada(arquivoImportado);
            importacaoEfetuada.AdicionarDadosPlanilha(dadosPlanilha);
            await _servicoArquivos.AddAsync(importacaoEfetuada);
            await _servicoArquivos.CommitAsync();
            await AtualizarContatosViaPlanilha(dadosPlanilha);
            await _servicoEstabelecimento.AtualizarEstabelecimentoViaPlanilha(dadosPlanilha);
            //await _servicoEstabelecimento.AtualizarAssociacaoEstabelecimentoContato(dadosPlanilha);
            await _servicoContato.AtualizarAssociacaoContatoEstabelecimento(dadosPlanilha);
            await _servicoEstabelecimento.AtualizarAssociacaoEstabelecimentoRede(dadosPlanilha);
            await _servicoEstabelecimento.AtualizarExtratosViaPlanilha(dadosPlanilha);
            return true;
        }

        private async Task AtualizarContatosViaPlanilha(List<DadosPlanilha> dadosPlanilha)
        {
            var contatosCadastrados = await _unitOfWork.repositorioContato.GetAllAsync(1, 999999);
            List<Contato> contatosPlanilha = new List<Contato>();
            foreach (var contato in dadosPlanilha.Select(x => x.Cnpj).Distinct().ToList())
            {
                contatosPlanilha.Add(new Contato(contato)
                {
                    OrigemContato = OrigemContato.PlanilhaIncidencia
                });
            }
            var contatosNaoCadastrados = contatosPlanilha.Except(contatosCadastrados.Dados).ToList();
            await _unitOfWork.repositorioContato.AddRangeAsync(contatosNaoCadastrados);
            await _unitOfWork.CommitAsync();
        }
    }
}