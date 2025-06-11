using Marketing.Domain.Extensoes;

namespace Marketing.Domain.Entidades
{
    public class Estabelecimento
    {
        public string Cnpj { get; set; } = String.Empty;
        public string RedeNome { get; set; } = String.Empty;
        public virtual Rede Rede { get; set; } = new Rede("");
        public string RazaoSocial { get; set; } = String.Empty;
        public string Cidade { get; set; } = String.Empty;
        public string Uf { get; set; } = String.Empty;
        public ICollection<Contato> Contatos { get; } = [];
        public ICollection<ExtratoVendas> ExtratoVendas { get; private set; } = new List<ExtratoVendas>();
        public string MesCompetencia => $"{this.ExtratoMesCompetencia.Competencia.
                                                ToString("MMMM yyyy").ToUpper()}";
        public ExtratoVendas ExtratoMesCompetencia => this.ExtratoVendas.
                                                     OrderByDescending(x => x.Competencia).
                                                     ElementAt(0);
        public decimal IncidenciaMedia
        {
            get { return (decimal)this.ExtratoVendas.Average(x => x.IncidenciaReal); }
        }
        public Estabelecimento()
        {
        }

        public void AdicionarExtrato(ExtratoVendas extratoVenda)
        {
            this.ExtratoVendas.Add(extratoVenda);
        }
        public void AdicionarExtratos(List<ExtratoVendas> extratosVenda)
        {
            foreach (var extrato in extratosVenda)
            {
                this.ExtratoVendas.Add(extrato);
            }
        }
        
        public string Periodo()
        { 
            if (this.ExtratoVendas.Count == 0)
            {
                return "";
            }
            else
            {
                if (ExtratoVendas.First() == null) return "";
                string mesDe = $"{ExtratoVendas.First().Competencia.ToString("MMM/yyyy").ToUpper()}";
                string mesAte = $"{ExtratoMesCompetencia.Competencia.ToString("MMM/yyyy").ToUpper()}";
                return $"{mesDe} A {mesAte}";
            }
        }
    }
}