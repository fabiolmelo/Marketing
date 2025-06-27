using System;
using System.Collections.Generic;

namespace Marketing.Domain.Entidades
{
    public class ImportacaoEfetuada
    {
        public int Id { get; set; }
        public string? NomeArquivoServer { get; set; } = String.Empty; 
        public DateTime DataImportacao { get; private set; } = DateTime.UtcNow; 
        public List<DadosPlanilha> DadosPlanilha { get; private set; } = new List<DadosPlanilha>();
        public ImportacaoEfetuada()
        {
        }

        public ImportacaoEfetuada(string nomeArquivo)
        {
            NomeArquivoServer = nomeArquivo;
        }

        public void AdicionarDadosPlanilha(DadosPlanilha dadosPlanilha){
            this.DadosPlanilha.Add(dadosPlanilha);
        }
        public void AdicionarDadosPlanilha(List<DadosPlanilha> dadosPlanilha){
            foreach(DadosPlanilha dado in dadosPlanilha){
                this.DadosPlanilha.Add(dado);
            }
        }
    }
}