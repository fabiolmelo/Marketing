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
            var caminhoPdf = Path.Combine(contentRootPath, "DadosApp", "Fechamentos",
                                          $"{arquivoPdf}");
            using (var image = File.OpenRead(caminhoFundo))
            {
                if (File.Exists(caminhoPdf)) File.Delete(caminhoPdf);
                using (FileStream filestream = new FileStream(caminhoPdf, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    Document document = new Document(PageSize.A4);
                    var worker = PdfWriter.GetInstance(document, filestream);
                    document.Open();

                    //GRAVA O FUNDO NO ARQUIVO
                    var pic = Image.GetInstance(caminhoFundo);
                    pic.SetAbsolutePosition(0, 0);
                    pic.ScaleToFit(document.PageSize);
                    document.Add(pic);

                    var chunckDadosEstabelecimento = new Chunk();

                    // DADOS DO ESTABELECIMENTO
                    var directContent = worker.DirectContent;
                    ColumnText columnText = new ColumnText(directContent);
                    Phrase phrase = new Phrase();
                    columnText.SetSimpleColumn(phrase, 50, 250, 580, 317, 15, Element.ALIGN_LEFT);
                    Paragraph razaoSocial = new Paragraph(estabelecimento.RazaoSocial, FontFactory.GetFont("Calibri", 10, BaseColor.BLUE));
                    razaoSocial.Add(chunckDadosEstabelecimento);
                    columnText.AddText(razaoSocial);
                    columnText.Go();

                    
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