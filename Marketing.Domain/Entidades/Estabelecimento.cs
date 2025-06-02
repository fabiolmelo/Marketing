using Marketing.Domain.Extensoes;

namespace Marketing.Domain.Entidades
{
    public class Estabelecimento 
    {
        public string Cnpj { get; set; } = String.Empty; 
        public int? RedeId { get; set; }
        public virtual Rede? Rede { get; set; }

        public string RazaoSocial { get; set; } = String.Empty;
        public string Cidade { get; set; } = String.Empty; 
        public string Uf { get; set; } = String.Empty; 
        public ICollection<Contato> Contatos { get; } = [];
        public ICollection<ExtratoVendas> ExtratoVendas { get; private set; } = new List<ExtratoVendas>();
        public string MesCompetencia => $"{this.ExtratoMesCompetencia.Competencia.
                                                ToString("MMMM yyyy").PriMaiuscula()}"; 
        public ExtratoVendas ExtratoMesCompetencia => this.ExtratoVendas.
                                                     OrderByDescending(x=>x.Mes).
                                                     ElementAt(0); 
        public decimal IncidenciaMedia { 
            get 
            { 
                return this.ExtratoVendas.Count == 0 ? 0 : this.ExtratoVendas.Sum(x => x.PedidosComCocaCola) / 
                           this.ExtratoVendas.Sum(x => x.TotalPedidos);
            }
        } 
        public Estabelecimento()
        {
        }

        public void AdicionarExtrato(ExtratoVendas extratoVenda){
            this.ExtratoVendas.Add(extratoVenda);
        }
        public void AdicionarExtratos(List<ExtratoVendas> extratosVenda){
            foreach(var extrato in extratosVenda){
                this.ExtratoVendas.Add(extrato);
            }
        }
    }
}