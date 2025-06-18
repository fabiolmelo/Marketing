using Marketing.Graphs.Entidades;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace Marketing.Graphs
{
    public static class ServicoGraph 
    {
        public static Chart GerarGrafico(string pathArquivo)
        {
            Estabelecimento estabelecimento = new Estabelecimento();

            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2025, 2, DateTime.Parse("01/02/2025"), 1334, 1071, (decimal)0.45, (decimal)0.8,
                              (decimal)13.75, -4, (decimal)-52.25, "29683855000146"));
            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2025, 1, DateTime.Parse("01/01/2025"), 1643, 1306, (decimal)0.95, (decimal)0.8,
                                  (decimal)13.75, 8, (decimal)115.50, "29683855000146"));
            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2024, 12, DateTime.Parse("01/12/2024"), 3172, 2703, (decimal)0.8, (decimal)0.8,
                                  (decimal)13.75, 159, (decimal)2180.75, "29683855000146"));
            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2024, 11, DateTime.Parse("01/11/2024"), 1334, 1071, (decimal)0.9528, (decimal)0.80,
                                  (decimal)13.75, -4, (decimal)-52.25, "29683855000146"));
            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2024, 10, DateTime.Parse("01/10/2024"), 1643, 1306, (decimal)0.7949, (decimal)0.80,
                                  (decimal)13.75, 8, (decimal)115.50, "29683855000146"));
            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2024, 9, DateTime.Parse("01/09/2024"), 3172, 2703, (decimal)0.8521, (decimal)0.8,
                                  (decimal)13.75, 159, (decimal)2180.75, "29683855000146"));
            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2024, 8, DateTime.Parse("01/08/2024"), 3172, 2703, (decimal)0.7521, (decimal)0.8,
                                  (decimal)13.75, 159, (decimal)2180.75, "29683855000146"));
            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2024, 7, DateTime.Parse("01/07/2024"), 3172, 2703, (decimal)0.8521, (decimal)0.8,
                                  (decimal)13.75, 159, (decimal)2180.75, "29683855000146"));
            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2024, 6, DateTime.Parse("01/06/2024"), 3172, 2703, (decimal)0.8521, (decimal)0.8,
                                  (decimal)13.75, 159, (decimal)2180.75, "29683855000146"));
            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2024, 5, DateTime.Parse("01/05/2024"), 3172, 2703, (decimal)0.8521, (decimal)0.8,
                                  (decimal)13.75, 159, (decimal)2180.75, "29683855000146"));
            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2024, 4, DateTime.Parse("01/04/2024"), 3172, 2703, (decimal)0.8521, (decimal)0.8,
                                  (decimal)13.75, 159, (decimal)2180.75, "29683855000146"));
            estabelecimento.ExtratoVendas.Add(
                new ExtratoVendas(2024, 3, DateTime.Parse("01/03/2024"), 3172, 2703, (decimal)0.8521, (decimal)0.8,
                                  (decimal)13.75, 159, (decimal)2180.75, "29683855000146"));
            
            var extratos = estabelecimento.ExtratoVendas.OrderByDescending(x => x.Competencia).ToList();

            using (Chart grafico = new Chart())
            {
                grafico.FormatNumber += Grafico_FormatNumber;
                grafico.Size = new Size(600, 268);

                ChartArea chartArea = new ChartArea("charArea1");
                grafico.Dock = System.Windows.Forms.DockStyle.Fill;
                chartArea.AlignmentOrientation = AreaAlignmentOrientations.All;
                chartArea.BorderWidth = 0;
                chartArea.AxisX.IsMarginVisible = false;
                chartArea.AxisX.LineWidth = 0;

                Series serieVenda = new Series("Venda");
                Series serieTransparente = new Series("Transparente");
                Series serieAcima = new Series("Acima");
                Series serieTraco = new Series("Traco");
                serieTraco.ChartType = SeriesChartType.Line;

                foreach (var extrato in extratos)
                {
                    var anoMes = extrato.Ano * 100 + extrato.Mes;
                    serieVenda.Points.AddXY(anoMes.ToString(), (double)extrato.CorVermelhaGrafico);
                    serieTransparente.Points.AddXY(anoMes.ToString(), (double)extrato.CorVerdeGrafico );
                    //serieAcima.Points.AddXY(anoMes.ToString(), (double)extrato.CorVerdeGrafico);
                    serieTraco.Points.AddY(extrato.Meta);
                }

                serieVenda.IsXValueIndexed = false; 
                serieVenda.Font = new Font("Verdana", 10, FontStyle.Bold);
                serieVenda.Color = Color.Red;
                serieVenda.IsValueShownAsLabel = false;
                serieVenda.LabelForeColor = Color.White;
                serieVenda.YValueType = ChartValueType.Double;
                serieVenda.ChartType = SeriesChartType.StackedColumn;
                serieVenda.LabelFormat = "{P0}";
               

                serieTransparente.Color = Color.Green   ;
                serieTransparente.IsValueShownAsLabel = false;
                serieTransparente.LabelForeColor = Color.White;
                serieTransparente.YValueType = ChartValueType.Double;
                serieTransparente.ChartType = SeriesChartType.StackedColumn;

                serieAcima.Color = Color.Green;
                serieAcima.IsValueShownAsLabel = false;
                serieAcima.LabelForeColor = Color.White;
                serieAcima.YValueType = ChartValueType.Double;
                serieVenda.ChartType = SeriesChartType.StackedColumn;

                serieTraco.Color = Color.Black;

                grafico.ChartAreas.Add(chartArea);
               
                grafico.Series.Add(serieVenda);
                grafico.Series.Add(serieTransparente);

                grafico.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                grafico.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                grafico.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
                grafico.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;


                grafico.ChartAreas[0].AxisY.Minimum = 0;
                grafico.SaveImage(pathArquivo, ChartImageFormat.Jpeg);
                return grafico;
            }
        }
        private static void Grafico_FormatNumber(object sender, FormatNumberEventArgs e)
        {

        }
    }
}
