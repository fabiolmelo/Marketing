using System.Text;
using Marketing.Domain.Extensoes;

namespace Marketing.Domain.Entidades
{
    public class Estabelecimento
    {
        public string Cnpj { get; set; } 
        public string RedeNome { get; set; }
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

        public string EnderecoCompleto()
        {
            var enderecoCompleto = new StringBuilder();
            var enderecoPartes  = ($"{this.Endereco}, {this.Numero} - {this.Complemento}").ChunkString(30);
            enderecoCompleto.AppendLine($"Loja: {this.RazaoSocial}");
            enderecoCompleto.AppendLine($"Cidade: {this.Cidade} - {this.Uf}");
            enderecoCompleto.AppendLine($"Endereço: {enderecoPartes.ElementAt(0)}");
            for(int index = 1; index < enderecoPartes.Count(); index++)
            {
                enderecoCompleto.AppendLine($"{enderecoPartes.ElementAt(index)}");
            }
            return enderecoCompleto.ToString();
        }
        public decimal IncidenciaMedia
        {
            get
            {
                return this.ExtratoVendas.Count == 0 ? 0 :
                (decimal)this.ExtratoVendas.OrderByDescending(x => x.Competencia).ElementAt(0).IncidenciaReal;
            }
        }

        public Estabelecimento(string cnpj, string redeNome)
        {
            Cnpj = cnpj;
            RedeNome = redeNome;
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
            return this.Cnpj == other.Cnpj && this.RedeNome == other.RedeNome;
        }

        public override int GetHashCode()
        {
            var chave = String.Join(Cnpj,RedeNome);
            return chave?.GetHashCode(StringComparison.Ordinal) ?? 0;

        }
    }
}