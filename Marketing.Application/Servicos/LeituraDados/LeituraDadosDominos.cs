using Marketing.Domain.DTOs;
using Marketing.Domain.Entidades;
using Marketing.Domain.Extensoes;
using Microsoft.IdentityModel.Tokens;

namespace Marketing.Application.Servicos.LeituraDados
{
    public class LeituraDadosDominos : LeituraDados
    {
        public override ResponseValidation LerDados(string pathArquivo)
        {
            int row = 2;
            string coluna = "";
            var responseValidation = new ResponseValidation();
            
            using(var excel = new ClosedXML.Excel.XLWorkbook(pathArquivo))
            {
                var planilha = excel.Worksheet(1).RowsUsed();
                var rede = "DOMINOS";

                DateTime data = DateTime.MaxValue;
                String uf = String.Empty;
                String cidade = String.Empty;
                String cnpj  = String.Empty;
                String restaurante  = String.Empty;;
                int totalPedidos = 0;
                int qtdPedidosNaoCapiturados = 0;
                int totalPedidosCoca = 0;
                decimal precoUnitarioMedio = 0;
                decimal incidencia = 0;
                decimal meta = 0;
                decimal receitaNaoCapturada = 0;
                string planilhaName = excel.Worksheet(1).Name ?? ""; 

                foreach (var linha in planilha)
                {
                    if (linha.RowNumber() == 1 && linha.Cell("D").Value.ToString() != "ID_LOJA")
                    {
                        responseValidation.FormatoInvalido = true;
                        return responseValidation;
                    }
                    if (linha.RowNumber() > 1 && !linha.IsEmpty())
                    {
                        try
                        {
                            coluna = "AnoMes";
                            ClosedXML.Excel.IXLCell cell = linha.Cell("A");
                            cell.Style.DateFormat.Format = "dd/MM/yyyy";
                            data = cell.GetValue<DateTime>();
                        }
                        catch (Exception)
                        {
                            responseValidation.AdicionarErro(planilhaName, row, coluna, "Data em formato inválido", "AnoMes precisa estar no formato DD/MM/AAAA");
                        }
                        try
                        {
                            coluna = "UF";
                            uf = linha.Cell("B").Value.ToString().ToUpper().Trim();
                            if (uf.Length != 2) responseValidation.AdicionarErro(planilhaName, row, coluna, "UF inválida em branco!");
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro(planilhaName, row, coluna, "Dados inválidos", ex.Message);
                        }
                        try
                        {
                            coluna = "Cidade";
                            cidade = linha.Cell("C").Value.ToString().ToUpper().Trim();
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro(planilhaName, row, coluna, "Dados inválidos", ex.Message);
                        }
                        try
                        {
                            coluna = "CNPJ";
                            cnpj = "00000" + linha.Cell("E").Value.ToString().RemoverCaracteresEspeciais();
                            cnpj = cnpj.Right(14);
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro(planilhaName, row, coluna, "Dados inválidos", ex.Message);
                        }
                        try
                        {
                            coluna = "Restaurante";
                            restaurante = linha.Cell("F").Value.ToString().ToUpper().Trim();
                            if (restaurante.IsNullOrEmpty()) responseValidation.AdicionarErro(planilhaName, row, coluna, "Restaurante em branco!");
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro(planilhaName, row, coluna, "Dados inválidos", ex.Message);
                        }
                        try
                        {
                            coluna = "TotalPedidos";
                            totalPedidos = (int)linha.Cell("G").Value.GetNumber();
                            if (totalPedidos == 0) responseValidation.AdicionarErro(planilhaName, row, coluna, "TotalPedidos zerado!");;
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro(planilhaName, row, coluna, "Dados inválidos", ex.Message);
                        }
                        try
                        {
                            coluna = "TotalPedidosCoca";
                            totalPedidosCoca = (int)linha.Cell("H").Value.GetNumber();
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro(planilhaName, row, coluna, "Dados inválidos",ex.Message);
                        }
                        try
                        {
                            coluna = "Incidencia";
                            incidencia = (decimal)linha.Cell("I").Value.GetNumber();
                            if (totalPedidosCoca > 0 && incidencia == 0)  throw new Exception("Incidência zerada!");
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro(planilhaName, row, coluna, "Dados inválidos",ex.Message);
                        }
                        try
                        {
                            coluna = "Meta";
                            meta = (decimal)linha.Cell("J").Value.GetNumber();
                            if (meta == 0) throw new Exception("Meta não preenchida!");
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro(planilhaName, row, coluna, "Dados inválidos",ex.Message);
                        }
                        try
                        {
                            coluna = "Preço Unitário";
                            precoUnitarioMedio = (decimal)linha.Cell("K").Value.GetNumber();
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro(planilhaName, row, coluna, "Dados inválidos",ex.Message);
                        }
                        try
                        {
                            coluna = "Qtde pedidos não capturados";
                            qtdPedidosNaoCapiturados = (int)linha.Cell("L").Value;
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro(planilhaName, row, coluna, "Dados inválidos", ex.Message);
                        }
                        try
                        {
                            coluna = "Receita não capturada";
                            receitaNaoCapturada = (decimal)linha.Cell("M").Value.GetNumber();
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro(planilhaName, row, coluna, "Dados inválidos", ex.Message);
                        }                            
                        responseValidation.AdicionarDados(
                            new DadosPlanilha(data, uf, cidade, cnpj, restaurante, totalPedidos, 
                                totalPedidosCoca, incidencia, meta, precoUnitarioMedio, 
                                qtdPedidosNaoCapiturados, receitaNaoCapturada, rede, "", row, planilhaName)
                        );
                        row++;
                    }
                }
            }
            return responseValidation;
        }
    }
}