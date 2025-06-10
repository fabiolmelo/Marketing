namespace Marketing.Domain.Entidades
{
    public class ExtratoVendas
    {
        public int Ano { get; set; }        
        public int Mes { get; set; }        
        public DateTime Competencia { get; set; } 
        public int TotalPedidos { get; set; }
        public int PedidosComCocaCola { get; set; }
        public decimal IncidenciaReal { get; set; }    
        public decimal Meta { get; set; }   
        public decimal PrecoUnitarioMedio { get; set; }   
        public int TotalPedidosNaoCapturados { get; set; }    
        public decimal ReceitaNaoCapturada { get; set; }
        public string EstabelecimentoCnpj { get; set; }
        public virtual Estabelecimento Estabelecimento { get; set; } = new Estabelecimento();
        public decimal CorVerdeGrafico { get; set; }
        public decimal CorTransparenteGrafico { get; set;}
        public decimal CorVermelhaGrafico { get; set; }
        public ExtratoVendas(int ano, int mes, DateTime competencia, int totalPedidos, int pedidosComCocaCola,
            decimal incidenciaReal, decimal meta, decimal precoUnitarioMedio, int totalPedidosNaoCapturados,
            decimal receitaNaoCapturada, string estabelecimentoCnpj)
        {
            Ano = ano;
            Mes = mes;
            Competencia = competencia;
            TotalPedidos = totalPedidos;
            PedidosComCocaCola = pedidosComCocaCola;
            IncidenciaReal = incidenciaReal;
            Meta = meta;
            PrecoUnitarioMedio = precoUnitarioMedio;
            TotalPedidosNaoCapturados = totalPedidosNaoCapturados;
            ReceitaNaoCapturada = receitaNaoCapturada;
            EstabelecimentoCnpj = estabelecimentoCnpj;

            CorVerdeGrafico = Meta > IncidenciaReal ? 0 : IncidenciaReal - Meta;
            CorTransparenteGrafico = IncidenciaReal > Meta ? 0 : Meta - IncidenciaReal;
            CorVermelhaGrafico =  IncidenciaReal > Meta ? Meta : IncidenciaReal; 
        }
    }
}