using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.UnitOfWork;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Http;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Marketing.Application.Servicos
{
    public class ServicoArquivo : Servico<ImportacaoEfetuada>, IServicoArquivos
    {
        public ServicoArquivo(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<string?> UploadArquivo(IFormFile arquivo)
        {
            if (arquivo == null){
                return null;
            }
            var arquivoImportado = $"{Guid.NewGuid().ToString()}.xlsx"; 
            var filePath = Path.Combine("DadosApp","Planilhas", arquivoImportado);
            try
            {
                using (FileStream filestream = System.IO.File.Create(filePath)){
                    await arquivo.CopyToAsync(filestream);
                    filestream.Flush();
                }
            }
            catch (System.Exception)
            {
                return null;
            }
            return filePath;
        }
        public string GerarArquivoPdf(Estabelecimento estabelecimento, string arquivoPdf,
                                      int posicao, String contentRootPath)
        {
            var caminhoFundo = Path.Combine(contentRootPath, "DadosApp", "CocaColaFundo.jpeg");
            var caminhoPdf = Path.Combine("wwwroot", "images", $"{arquivoPdf}");
            using (var image = File.OpenRead(caminhoFundo))
            {
                if (File.Exists(caminhoPdf)) File.Delete(caminhoPdf);
                using (FileStream filestream = new FileStream(caminhoPdf, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    Document document = new Document(PageSize.A4);
                    var worker = PdfWriter.GetInstance(document, filestream);
                    document.Open();

                    // FONTES
                    Font fontDadosEstabelecimento = FontFactory.GetFont("Calibri", 6, Font.NORMAL, BaseColor.WHITE);
                    Font fontPosicaoRede = FontFactory.GetFont("Arial Black", 28, Font.BOLD, BaseColor.WHITE);
                    Font fontMesReferencia = FontFactory.GetFont("TCCC Unity", 12, Font.BOLD, BaseColor.BLACK);
                    Font fontVendas = FontFactory.GetFont("TCCC Unity", 14, Font.NORMAL, BaseColor.BLACK);

                    //GRAVA O FUNDO NO ARQUIVO
                    var pic = Image.GetInstance(caminhoFundo);
                    pic.SetAbsolutePosition(0, 0);
                    pic.ScaleToFit(document.PageSize);
                    document.Add(pic);

                    // DADOS DO ESTABELECIMENTO
                    var dadosEstabelecimento1 = $"Franqueado: {estabelecimento.RazaoSocial}";
                    var dadosEstabelecimento2 = $"Cidade: {estabelecimento.Cidade} - {estabelecimento.Uf}";
                    var dadosEstabelecimento3 = $"Endereço: ";
                    var dadosEstabelecimento4 = $"Código:        CO";

                    //DADOS 1
                    PdfContentByte directContent = worker.DirectContent;
                    ColumnText columnText = new ColumnText(directContent);
                    var posicaoDados = new Phrase(new Chunk(dadosEstabelecimento1, fontDadosEstabelecimento)); 
                    columnText.SetSimpleColumn(posicaoDados, 450, 100, 50, 700, 25, Element.ALIGN_LEFT | Element.ALIGN_LEFT);
                    columnText.Go();

                    //DADOS 2
                    ColumnText columnText2 = new ColumnText(directContent);
                    var posicaoDados2 = new Phrase(new Chunk(dadosEstabelecimento2, fontDadosEstabelecimento)); 
                    columnText2.SetSimpleColumn(posicaoDados2, 450, 100, 50, 690, 25, Element.ALIGN_LEFT | Element.ALIGN_LEFT);
                    columnText2.Go();

                    //DADOS 3
                    ColumnText columnText3 = new ColumnText(directContent);
                    var posicaoDados3 = new Phrase(new Chunk(dadosEstabelecimento3, fontDadosEstabelecimento)); 
                    columnText3.SetSimpleColumn(posicaoDados3, 450, 100, 50, 680, 25, Element.ALIGN_LEFT | Element.ALIGN_LEFT);
                    columnText3.Go();

                    //DADOS 4
                    ColumnText columnText4 = new ColumnText(directContent);
                    var posicaoDados4 = new Phrase(new Chunk(dadosEstabelecimento4, fontDadosEstabelecimento)); 
                    columnText4.SetSimpleColumn(posicaoDados4, 450, 100, 50, 670, 25, Element.ALIGN_LEFT | Element.ALIGN_LEFT);
                    columnText4.Go();

                    //MES REFERENCIA
                    string textoMesReferencia = $"{estabelecimento.MesCompetencia} (META DE INCIDÊNCIA: ";
                    textoMesReferencia += $"{(int)(estabelecimento.ExtratoMesCompetencia.Meta * 100)}%)";
                    ColumnText mesReferencia = new ColumnText(directContent);
                    var mesReferenciaPhrase = new Phrase(new Chunk(textoMesReferencia, fontMesReferencia)); 
                    mesReferencia.SetSimpleColumn(mesReferenciaPhrase, 350, 50, 5, 635, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    mesReferencia.Go();


                    //TOTAL DE PEDIDOS
                    ColumnText totalPedido = new ColumnText(directContent);
                    string totalPedidos = estabelecimento.ExtratoMesCompetencia.TotalPedidos.ToString("N0");
                    var totalPedidoPhrase = new Phrase(new Chunk(totalPedidos, fontVendas)); 
                    totalPedido.SetSimpleColumn(totalPedidoPhrase, 100, 560, 25, 595, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    totalPedido.Go();

                    //TOTAL DE PEDIDOS COM COCA
                    ColumnText totalPedidoCoca = new ColumnText(directContent);
                    string totalPedidosCoca = estabelecimento.ExtratoMesCompetencia.PedidosComCocaCola.ToString("N0");
                    var totalPedidoCocaPhrase = new Phrase(new Chunk(totalPedidosCoca, fontVendas)); 
                    totalPedidoCoca.SetSimpleColumn(totalPedidoCocaPhrase, 190, 560, 105, 595, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    totalPedidoCoca.Go();


                    // DADOS DA POSICAO
                    var posicaoTexto = $"{posicao.ToString()}º";
                    PdfContentByte cb = worker.DirectContent;
                    ColumnText ct = new ColumnText(cb);
                    var posicaoPhrase = new Phrase(new Chunk($"{posicao.ToString()}º", fontPosicaoRede)); 
                    ct.SetSimpleColumn(posicaoPhrase,  920, 100, 50, 580, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    ct.Go();

                    // FUTURO TEXTO EXPLICATIVO
                    document.NewPage();

                    filestream.Flush();
                    document.CloseDocument();
                }
             }
            return arquivoPdf; 
        }

        public List<DadosPlanilha> LerDados(string pathArquivo)
        {
            int row = 2;
            var dadosPlanilha = new List<DadosPlanilha>();

            using(var excel = new ClosedXML.Excel.XLWorkbook(pathArquivo))
            {
                var planilha = excel.Worksheet(1).RowsUsed();
                foreach (var linha in planilha)
                {
                    if (linha.RowNumber() > 1 && !linha.IsEmpty())
                    {
                        var data = linha.Cell("A").Value.GetDateTime();
                        var uf = linha.Cell("B").Value.ToString();
                        var cidade = linha.Cell("C").Value.ToString();
                        var cnpj = linha.Cell("D").Value.ToString();
                        var restaurante = linha.Cell("E").Value.ToString();
                        var totalPedidos = (int)linha.Cell("F").Value.GetNumber();
                        var totalPedidosCoca = (int)linha.Cell("G").Value.GetNumber();
                        var incidencia = (decimal)linha.Cell("H").Value.GetNumber();
                        var meta = (decimal)linha.Cell("I").Value.GetNumber();
                        var precoUnitarioMedio = (decimal)linha.Cell("J").Value.GetNumber();
                        var qtdPedidosNaoCapiturados = (int)linha.Cell("K").Value;
                        var receitaNaoCapturada = (decimal)linha.Cell("L").Value.GetNumber();
                        var rede = linha.Cell("M").Value.ToString();
                        var fone = linha.Cell("N").Value.ToString();

                        dadosPlanilha.Add(
                            new DadosPlanilha(data, uf, cidade, cnpj, restaurante, totalPedidos, 
                                totalPedidosCoca, incidencia, meta, precoUnitarioMedio, 
                                qtdPedidosNaoCapiturados, receitaNaoCapturada, rede, fone)
                        );
                    }
                    row++;
                }
            }
            return dadosPlanilha;
        }
    }
}