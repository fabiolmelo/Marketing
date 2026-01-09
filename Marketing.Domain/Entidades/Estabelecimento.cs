namespace Marketing.Domain.Entidades
{
    public class Estabelecimento
    {
        public string Cnpj { get; set; } = null!;
        public string? RedeNome { get; set; }
        public virtual Rede? Rede { get; set; }
        public string RazaoSocial { get; set; } = null!;
        
        public string? Endereco { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; } 
        public string? Uf { get; set; } 
        public string? Cep { get; set; } 
        public ICollection<ContatoEstabelecimento> ContatoEstabelecimentos { get; set; } = null!;
        public List<ExtratoVendas> ExtratoVendas { get; set; } = new List<ExtratoVendas>();
        public string MesCompetencia => $"{this.ExtratoMesCompetencia.Competencia.ToString("MMMM yyyy").ToUpper()}";
        public ExtratoVendas ExtratoMesCompetencia => this.ExtratoVendas.OrderByDescending(x => x.Competencia).ElementAt(0);
        public string? UltimoPdfGerado { get; set; }
        // public ICollection<EnvioMensagemMensal> MensagensMensais { get; set; } = new List<EnvioMensagemMensal>();
        public decimal IncidenciaMedia
        {
            //get { return this.ExtratoVendas.Count == 0 ? 0 : (decimal)this.ExtratoVendas.Average(x => x.IncidenciaReal); }
            get
            {
                return this.ExtratoVendas.Count == 0 ? 0 :
                (decimal)this.ExtratoVendas.OrderByDescending(x => x.Competencia).ElementAt(0).IncidenciaReal;
            }
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

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (this == obj) return true;
            var other = (Estabelecimento)obj;
            return this.Cnpj == other.Cnpj;
        }
    }
}