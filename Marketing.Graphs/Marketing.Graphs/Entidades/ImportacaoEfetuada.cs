using System;
using System.Collections.Generic;

namespace Marketing.Graphs.Entidades
{
    public class ImportacaoEfetuada
    {
        public int Id { get; set; } 
        public string NomeArquivoServer  { get; set; } 
        public DateTime DataImportacao { get; private set; } = DateTime.UtcNow; 
        public ICollection<DadosPlanilha> DadosPlanilha { get; private set; } = new List<DadosPlanilha>();
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