namespace Marketing.Domain.Entidades
{
    public class DadosPlanilha
    {
        public int ImportacaoEfetuadaId { get; set; } = 0;
        public virtual ImportacaoEfetuada ImportacaoEfetuada { get; set; }
        public DateTime AnoMes { get; set; }
        public string Uf { get; set; }
        public string Cidade { get; set; }
        public string Cnpj { get; set; }
        public string Restaurante { get; set; }
        public int TotalPedidos { get; set; }
        public int PedidosComCocaCola { get; set; }
        public decimal IncidenciaReal { get; set; }    
        public decimal Meta { get; set; }   
        public decimal PrecoUnitarioMedio { get; set; }   
        public int TotalPedidosNaoCapturados { get; set; }    
        public decimal ReceitaNaoCapturada { get; set; }
        public string Rede { get; set; } 
        public string Fone { get; set; } 
        public int? Linha { get; set; }
        public string? Planilha { get; set; }

        public DadosPlanilha(DateTime anoMes, string uf, string cidade, string cnpj, string restaurante, int totalPedidos, 
                            int pedidosComCocaCola, decimal incidenciaReal, decimal meta, decimal precoUnitarioMedio, 
                            int totalPedidosNaoCapturados, decimal receitaNaoCapturada, string rede, string fone, 
                            int? linha, string? planilha)
        {
            AnoMes = anoMes;
            Uf = uf;
            Cidade = cidade;
            Cnpj = cnpj;
            Restaurante = restaurante;
            TotalPedidos = totalPedidos;
            PedidosComCocaCola = pedidosComCocaCola;
            IncidenciaReal = incidenciaReal;
            Meta = meta;
            PrecoUnitarioMedio = precoUnitarioMedio;
            TotalPedidosNaoCapturados = totalPedidosNaoCapturados;
            ReceitaNaoCapturada = receitaNaoCapturada;
            Rede = rede;
            Fone = fone;
            ImportacaoEfetuada = new ImportacaoEfetuada();
            Linha = linha;
            Planilha = planilha;
        }
    }
}