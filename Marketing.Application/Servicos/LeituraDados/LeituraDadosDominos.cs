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
                            data = linha.Cell("A").Value.GetDateTime();
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro("DOMINOS", row, coluna, "Dados inválidos", ex.Message);
                        }
                        try
                        {
                            coluna = "UF";
                            uf = linha.Cell("B").Value.ToString().ToUpper();
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro("DOMINOS", row, coluna, "Dados inválidos", ex.Message);
                        }
                        try
                        {
                            coluna = "Cidade";
                            cidade = linha.Cell("C").Value.ToString().ToUpper();
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro("DOMINOS", row, coluna, "Dados inválidos", ex.Message);
                        }
                        try
                        {
                            coluna = "CNPJ";
                            cnpj = "00000" + linha.Cell("E").Value.ToString().RemoverCaracteresEspeciais();
                            cnpj = cnpj.Right(14);
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro("DOMINOS", row, coluna, "Dados inválidos", ex.Message);
                        }
                        try
                        {
                            coluna = "Restaurante";
                            restaurante = linha.Cell("F").Value.ToString();
                            if (restaurante.IsNullOrEmpty()) responseValidation.AdicionarErro("DOMINOS", row, coluna, "Restaurante em branco!");
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro("DOMINOS", row, coluna, "Dados inválidos", ex.Message);
                        }
                        try
                        {
                            coluna = "TotalPedidos";
                            totalPedidos = (int)linha.Cell("G").Value.GetNumber();
                            if (totalPedidos == 0) throw new Exception("Total de pedidos zerado!");
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro("DOMINOS", row, coluna, "Dados inválidos", ex.Message);
                        }
                        try
                        {
                            coluna = "TotalPedidosCoca";
                            totalPedidosCoca = (int)linha.Cell("H").Value.GetNumber();
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro("DOMINOS", row, coluna, "Dados inválidos",ex.Message);
                        }
                        try
                        {
                            coluna = "Incidencia";
                            incidencia = (decimal)linha.Cell("I").Value.GetNumber();
                            if (totalPedidosCoca > 0 && incidencia == 0)  throw new Exception("Incidência zerada!");
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro("DOMINOS", row, coluna, "Dados inválidos",ex.Message);
                        }
                        try
                        {
                            coluna = "Meta";
                            meta = (decimal)linha.Cell("J").Value.GetNumber();
                            if (meta == 0) throw new Exception("Meta não preenchida!");
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro("DOMINOS", row, coluna, "Dados inválidos",ex.Message);
                        }
                        try
                        {
                            coluna = "Preço Unitário";
                            precoUnitarioMedio = (decimal)linha.Cell("K").Value.GetNumber();
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro("DOMINOS", row, coluna, "Dados inválidos",ex.Message);
                        }
                        try
                        {
                            coluna = "Qtde pedidos não capturados";
                            qtdPedidosNaoCapiturados = (int)linha.Cell("L").Value;
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro("DOMINOS", row, coluna, "Dados inválidos", ex.Message);
                        }
                        try
                        {
                            coluna = "Receita não capturada";
                            receitaNaoCapturada = (decimal)linha.Cell("M").Value.GetNumber();
                        }
                        catch (Exception ex)
                        {
                            responseValidation.AdicionarErro("DOMINOS", row, coluna, "Dados inválidos", ex.Message);
                        }                            
                        responseValidation.AdicioarDados(
                            new DadosPlanilha(data, uf, cidade, cnpj, restaurante, totalPedidos, 
                                totalPedidosCoca, incidencia, meta, precoUnitarioMedio, 
                                qtdPedidosNaoCapiturados, receitaNaoCapturada, rede, "")
                        );
                        row++;
                    }
                }
            }
            return responseValidation;
        }
    }
}