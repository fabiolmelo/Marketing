using ClosedXML.Excel;
using Marketing.Domain.DTOs;
using Marketing.Domain.Entidades;
using Marketing.Domain.Extensoes;

namespace Marketing.Application.Servicos.LeituraDados
{
    public class LeituraDadosGrupoTrigo : LeituraDados
    {
        public override ResponseValidation LerDados(string pathArquivo)
        {
            int row;
            string coluna;
            IXLRows planilha;
            var responseValidation = new ResponseValidation();
            
            using(var excel = new ClosedXML.Excel.XLWorkbook(pathArquivo))
            {
                for(int aba = 1; aba < excel.Worksheets.Count(); aba++)
                {
                    row=2;
                    coluna=String.Empty;
                    planilha = excel.Worksheet(aba).RowsUsed();

                    string rede = String.Empty;

                    DateTime data = DateTime.MaxValue;
                    String uf = String.Empty;
                    String cidade = String.Empty;
                    String cnpj  = String.Empty;
                    String restaurante  = String.Empty;
                    String telefone = String.Empty;
                    int totalPedidos = 0;
                    int qtdPedidosNaoCapiturados = 0;
                    int totalPedidosCoca = 0;
                    decimal precoUnitarioMedio = 0;
                    decimal incidencia = 0;
                    decimal meta = 0;
                    decimal receitaNaoCapturada = 0;
                    string planilhaName = excel.Worksheet(aba).Name ?? ""; 

                    foreach (var linha in planilha)
                    {
                        if (linha.RowNumber() == 1 && linha.Cell("B").Value.ToString() != "Operacao")
                        {
                            responseValidation.FormatoInvalido = true;
                            return responseValidation;
                        }
                        if (linha.RowNumber() > 1 && !linha.IsEmpty())
                        {                        
                            try
                            {
                                coluna = "AnoMes";
                                IXLCell cell = linha.Cell("A"); 
                                string dataValue = cell.Value.ToString();
                                if (dataValue.Length == 7)
                                {
                                    var dataString = cell.Value.ToString();
                                    var ano = int.Parse(dataString.Substring(0,4));
                                    var mes = int.Parse(dataString.Right(2));
                                    data = new DateTime(ano,mes,1);
                                }
                                else
                                {
                                    cell.Style.DateFormat.Format = "dd/MM/yyyy";
                                    data = cell.GetValue<DateTime>();
                                }                       
                            }
                            catch (Exception)
                            {
                                responseValidation.AdicionarErro(planilhaName, row, coluna, "Data em formato inválido", "AnoMes precisa estar no formato DD/MM/AAAA");
                            }
                            try
                            {
                                coluna = "Operacao";
                                rede = linha.Cell("B").Value.ToString().ToUpper().Trim();
                            }
                            catch (Exception ex)
                            {
                                responseValidation.AdicionarErro(planilhaName, row, coluna, "Dados inválidos", ex.Message);
                            }
                            try
                            {
                                coluna = "UF";
                                uf = linha.Cell("C").Value.ToString().ToUpper().Trim();
                                if (uf.Length != 2) responseValidation.AdicionarErro(planilhaName, row, coluna, "UF inválida em branco!");
                            }
                            catch (Exception ex)
                            {
                                responseValidation.AdicionarErro(planilhaName, row, coluna, "Dados inválidos", ex.Message);
                            }
                            try
                            {
                                coluna = "Cidade";
                                cidade = linha.Cell("D").Value.ToString().ToUpper().Trim();
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
                                if (string.IsNullOrEmpty(restaurante)) responseValidation.AdicionarErro(planilhaName, row, coluna, "Restaurante em branco!");
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
                                coluna = "Telefone";
                                telefone = linha.Cell("N").Value.ToString().Trim().RemoverCaracteresEspeciais();
                                if ((telefone.Length > 0 && telefone.Length < 10) || telefone.Length > 13)
                                {
                                    responseValidation.AdicionarErro(planilhaName, row, coluna, "Dados inválidos", 
                                    "Telefone precisa ter o formato +55(11)91234-5678 (podendo ser apenas números)");
                                }
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
                
            }
            return responseValidation;
        }
    }
}