using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketing.Domain.Entidades
{
    public class ExportacaoV1
    {
        public ExportacaoV1(DateTime anoMes, string uF, string cidade, string cNPJ, string restaurante, int pedidosTotal, int pedidosCoca, decimal incidenciaReal, decimal meta, decimal precoUnitario, int pedidosNaoCapiturados, decimal receitaBruta, string telefone)
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
            PedidosNaoCapiturados = pedidosNaoCapiturados;
            ReceitaBruta = receitaBruta;
            Telefone = telefone;
        }

        public DateTime AnoMes { get; private set; }	
        public string UF { get; private set; }	
        public string Cidade { get; private set; }	
        public string CNPJ { get; private set; }	
        public string Restaurante { get; private set; }	
        public int PedidosTotal { get; private set; }	
        public int PedidosCoca { get; private set; }	
        public decimal IncidenciaReal { get; private set; }	
        public decimal Meta { get; private set; }	 
        public decimal PrecoUnitario { get; private set; } 	
        public int PedidosNaoCapiturados { get; private set; }	 
        public decimal ReceitaBruta { get; private set; } 	
        public string Telefone { get; private set; }

    }
}