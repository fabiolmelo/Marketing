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

                foreach (var linha in planilha)
                {
                    try
                    {
                        if (linha.RowNumber() > 1 && !linha.IsEmpty())
                        {
                            coluna = "AnoMes";
                            var data = linha.Cell("A").Value.GetDateTime();
                            coluna = "UF";
                            var uf = linha.Cell("B").Value.ToString().ToUpper();
                            coluna = "Cidade";
                            var cidade = linha.Cell("C").Value.ToString().ToUpper();
                            coluna = "CNPJ";
                            var cnpj = "00000" + linha.Cell("E").Value.ToString().RemoverCaracteresEspeciais();
                            cnpj = cnpj.Right(14);
                            coluna = "Restaurante";
                            var restaurante = linha.Cell("F").Value.ToString();
                            if (restaurante.IsNullOrEmpty()) throw new Exception("Restaurante em branco!");
                            coluna = "TotalPedidos";
                            var totalPedidos = (int)linha.Cell("G").Value.GetNumber();
                            if (totalPedidos == 0) throw new Exception("Total de pedidos zerado!");
                            coluna = "TotalPedidosCoca";
                            var totalPedidosCoca = (int)linha.Cell("H").Value.GetNumber();
                            coluna = "Incidencia";
                            var incidencia = (decimal)linha.Cell("I").Value.GetNumber();
                            if (totalPedidosCoca > 0 && incidencia == 0)  throw new Exception("Incidência zerada!");
                            coluna = "Meta";
                            var meta = (decimal)linha.Cell("J").Value.GetNumber();
                            if (meta == 0) throw new Exception("Meta não preenchida!");
                            coluna = "Preço Unitário";
                            var precoUnitarioMedio = (decimal)linha.Cell("K").Value.GetNumber();
                            coluna = "Qtde pedidos não capturados";
                            var qtdPedidosNaoCapiturados = (int)linha.Cell("L").Value;
                            coluna = "Receita não capturada";
                            var receitaNaoCapturada = (decimal)linha.Cell("M").Value.GetNumber();
                            

                            responseValidation.AdicioarDados(
                                new DadosPlanilha(data, uf, cidade, cnpj, restaurante, totalPedidos, 
                                    totalPedidosCoca, incidencia, meta, precoUnitarioMedio, 
                                    qtdPedidosNaoCapiturados, receitaNaoCapturada, rede, "")
                            );
                        }
                        row++;
                    }
                    catch (Exception ex)
                    {
                        responseValidation.AdicionarErro("DOMINOS", row, coluna, "Dados inválidos",ex.Message);
                        row++;
                    }
                }
            }
            return responseValidation;
        }
    }
}