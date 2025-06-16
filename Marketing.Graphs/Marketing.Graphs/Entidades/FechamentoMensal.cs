using System;

namespace Marketing.Graphs.Entidades
{
    public class FechamentoMensal
    {
        public FechamentoMensal(DateTime competencia,
                                string estabelecimentoCnpj,
                                string pathPdf)
        {
            Competencia = competencia;
            EstabelecimentoCnpj = estabelecimentoCnpj;
            PathPdf = pathPdf;
        }

        public DateTime Competencia { get; set; }
        public string EstabelecimentoCnpj { get; set; }
        public virtual Estabelecimento Estabelecimento { get; set; } = new Estabelecimento();
        public string PathPdf { get; set; }
        
    }
}