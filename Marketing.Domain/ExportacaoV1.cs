namespace Marketing.Domain
{
    public class ExportacaoV1
    {
        public ExportacaoV1(string anoMes, string uF, string cidade, string cNPJ, string restaurante, int pedidosTotal, int pedidosCoca, decimal incidenciaReal, decimal meta, decimal precoUnitario, int qtdNaoCapiturada, decimal receitaBruta, string telefone)
        {
            AnoMes = anoMes;
            UF = uF;
            Cidade = cidade;
            CNPJ = cNPJ;
            Restaurante = restaurante;
            PedidosTotal = pedidosTotal;
            PedidosCoca = pedidosCoca;
            IncidenciaReal = incidenciaReal;
            Meta = meta;
            PrecoUnitario = precoUnitario;
            QtdNaoCapiturada = qtdNaoCapiturada;
            ReceitaBruta = receitaBruta;
            Telefone = telefone;
        }

        public String AnoMes { get; private set; }
        public String UF { get; private set; }
        public String Cidade { get; private set; }
        public String CNPJ { get; private set; }
        public String Restaurante { get; private set; }
        public int PedidosTotal { get; private set; }
        public int PedidosCoca { get; private set; }
        public decimal IncidenciaReal { get; private set; }
        public decimal Meta { get; private set; }
        public decimal PrecoUnitario { get; private set; }
        public int QtdNaoCapiturada { get; private set; }
        public decimal ReceitaBruta { get; private set; }
        public String Telefone { get; private set; }

    }
}