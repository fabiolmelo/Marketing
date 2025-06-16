using Marketing.Graphs.Entidades;
using System;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace Marketing.Graphs
{
    public static class ServicoGraph 
    {
        public static Chart GerarGrafico(string pathArquivo)
        {
            Estabelecimento estabelecimento = new Estabelecimento();

            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2025, 2, DateTime.Parse("01/02/2025"), 1334, 1071, (decimal)0.8028, (decimal)0.80,
                                  (decimal)13.75, -4, (decimal)-52.25, "29683855000146"));
            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2025, 1, DateTime.Parse("01/01/2025"), 1643, 1306, (decimal)0.7949, (decimal)0.80,
                                  (decimal)13.75, 8, (decimal)115.50, "29683855000146"));
            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2024, 12, DateTime.Parse("01/12/2024"), 3172, 2703, (decimal)0.8521, (decimal)0.9021,
                                  (decimal)13.75, 159, (decimal)2180.75, "29683855000146"));




            using (Chart grafico = new Chart())
            {
                grafico.FormatNumber += Grafico_FormatNumber;
                grafico.DataSource = estabelecimento.ExtratoVendas.ToArray();
                Series serieVenda = new Series("Venda");
                Series serieTransparente = new Series("Transparente");
                Series serieAcima = new Series("Acima");

                serieVenda.XValueMember = "Mes";
                serieVenda.Font = new Font("Verdana", 10, FontStyle.Bold);
                serieVenda.YValueMembers = "CorVermelhaGrafico";
                serieVenda.Color = Color.Red;
                serieVenda.IsValueShownAsLabel = true;
                serieVenda.LabelForeColor = Color.White;
                serieVenda.YValueType = ChartValueType.Double;
                serieVenda.SmartLabelStyle.Enabled = true;
                serieVenda.SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Bottom;
                serieVenda.ChartType = SeriesChartType.StackedColumn;

                serieTransparente.XValueMember = "Mes";
                serieTransparente.YValueMembers = "CorTransparenteGrafico";
                serieTransparente.Color = Color.Transparent;
                serieTransparente.IsValueShownAsLabel = false;
                serieTransparente.LabelForeColor = Color.White;
                serieTransparente.YValueType = ChartValueType.Double;
                serieTransparente.ChartType = SeriesChartType.StackedColumn;


                serieAcima.XValueMember = "Mes";
                serieAcima.YValueMembers = "CorVerdeGrafico";
                serieAcima.Color = Color.Green;
                serieAcima.IsValueShownAsLabel = false;
                serieAcima.LabelForeColor = Color.White;
                serieAcima.YValueType = ChartValueType.Double;
                serieVenda.ChartType = SeriesChartType.StackedColumn;

                grafico.Series.Add(serieVenda);
                grafico.Series.Add(serieTransparente);
                grafico.Series.Add(serieAcima);

                grafico.SaveImage(pathArquivo, ChartImageFormat.Jpeg);
                return grafico;
            }
        }
        private static void Grafico_FormatNumber(object sender, FormatNumberEventArgs e)
        {

        }
    }
}
