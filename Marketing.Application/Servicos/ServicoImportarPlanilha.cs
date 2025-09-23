using Marketing.Domain.Entidades;
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

        public ServicoImportarPlanilha(IServicoContato servicoContato,
                                       IServicoRede servicoRede,
                                       IServicoArquivos servicoArquivos,
                                       IServicoEstabelecimento servicoEstabelecimento,
                                       IServicoExtratoVendas servicoExtratoVendas)
        {
            _servicoContato = servicoContato;
            _servicoRede = servicoRede;
            _servicoArquivos = servicoArquivos;
            _servicoEstabelecimento = servicoEstabelecimento;
            _servicoExtratoVendas = servicoExtratoVendas;
        }

        public async Task<bool> ImportarPlanilha(IFormFile formFile)
        {
             var arquivoImportado = await _servicoArquivos.UploadArquivo(formFile);
            if (arquivoImportado == null)
                return false;
            var dadosPlanilha = _servicoArquivos.LerDados(arquivoImportado);
            var importacaoEfetuada = new ImportacaoEfetuada(arquivoImportado);
            importacaoEfetuada.AdicionarDadosPlanilha(dadosPlanilha);
            await _servicoArquivos.AddAsync(importacaoEfetuada);
            await _servicoRede.AtualizarRedesViaPlanilha(dadosPlanilha);
            await _servicoContato.AtualizarContatosViaPlanilha(dadosPlanilha);
            await _servicoEstabelecimento.AtualizarEstabelecimentoViaPlanilha(dadosPlanilha);
            await _servicoEstabelecimento.AtualizarAssociacaoEstabelecimentoContato(dadosPlanilha);
            await _servicoEstabelecimento.AtualizarAssociacaoEstabelecimentoRede(dadosPlanilha);
            await _servicoEstabelecimento.AtualizarExtratosViaPlanilha(dadosPlanilha);
            return true;
        }
    }
}