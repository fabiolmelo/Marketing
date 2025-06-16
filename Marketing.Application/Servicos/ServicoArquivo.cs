using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.UnitOfWork;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Http;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Marketing.Domain.Extensoes;

namespace Marketing.Application.Servicos
{
    public class ServicoArquivo : Servico<ImportacaoEfetuada>, IServicoArquivos
    {
        private readonly IServicoGrafico servicoGrafico;
        public ServicoArquivo(IUnitOfWork unitOfWork, IServicoGrafico servicoGrafico) : base(unitOfWork)
        {
            this.servicoGrafico = servicoGrafico;
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
            var caminhoFundo = Path.Combine(contentRootPath, "DadosApp", "fundo_export.png");
            var caminhoFontes = Path.Combine(contentRootPath, "DadosApp", "Fonts");
            //var caminhoFundo = Path.Combine(contentRootPath, "DadosApp", "CocaColaFundo.jpeg");
            var caminhoPdf = Path.Combine("wwwroot", "images", $"{arquivoPdf}");
            using (var image = File.OpenRead(caminhoFundo))
            {
                if (File.Exists(caminhoPdf)) File.Delete(caminhoPdf);
                using (FileStream filestream = new FileStream(caminhoPdf, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    Document document = new Document(PageSize.A4);
                    var worker = PdfWriter.GetInstance(document, filestream);
                    document.Open();

                    //REGISTRAR FONTES 
                    try
                    {
                        if (!FontFactory.IsRegistered("TCCCHolidays23-Curated")) FontFactory.Register(caminhoFontes + "\\TCCCHolidays23-Curated.otf");
                        if (!FontFactory.IsRegistered("TCCCHolidays23-Narrow")) FontFactory.Register(caminhoFontes + "\\TCCCHolidays23-Narrow.otf");
                        if (!FontFactory.IsRegistered("TCCCHolidays23-Normal")) FontFactory.Register(caminhoFontes + "\\TCCCHolidays23-Normal.otf");
                        if (!FontFactory.IsRegistered("TCCCUnity-Black")) FontFactory.Register(caminhoFontes + "\\TCCCUnity-Black.ttf");
                        if (!FontFactory.IsRegistered("TCCCUnity-Bold")) FontFactory.Register(caminhoFontes + "\\TCCCUnity-Bold.ttf");
                        if (!FontFactory.IsRegistered("TCCC-UnityCondensed-Bold")) FontFactory.Register(caminhoFontes + "\\TCCC-UnityCondensed-Bold.ttf");
                        if (!FontFactory.IsRegistered("TCCCUnity-Regular")) FontFactory.Register(caminhoFontes + "\\TCCCUnity-Regular.ttf");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    

                    // FONTES
                    var fontes = FontFactory.RegisteredFonts;
                    Font fontDadosEstabelecimento = FontFactory.GetFont("TCCC-UnityCondensed Bold", 6, Font.NORMAL, BaseColor.WHITE);
                    Font fontPosicaoRede = FontFactory.GetFont("TCCC Unity Black", 28, Font.NORMAL, BaseColor.WHITE);
                    Font fontMesReferencia = FontFactory.GetFont($@"{caminhoFontes}\TCCC-UnityCondensed-Bold.ttf", 12,
                                                            Font.NORMAL, BaseColor.BLACK);
                    
                    Font fontVendas = FontFactory.GetFont("TCCC-UnityText", 14, Font.NORMAL, BaseColor.BLACK);
                    Font fontVendasBold = FontFactory.GetFont("TCCC-UnityHeadline", 14, Font.NORMAL, BaseColor.BLACK);
                    Font fontVendasReceita = FontFactory.GetFont("TCCC-UnityText", 14, Font.NORMAL, BaseColor.RED);
                    Font fontMes = FontFactory.GetFont("tccc unity regular", 9, Font.NORMAL, BaseColor.BLACK);
                    Font fontValoresGraf = FontFactory.GetFont("tccc-unitycondensed-bold", 7, Font.NORMAL, BaseColor.BLACK);
                    Font fontValoresGrafRed = FontFactory.GetFont("tccc-unitycondensed-bold", 7, Font.NORMAL, BaseColor.RED);
                    Font fontValoresGrafGreen = FontFactory.GetFont("tccc-unitycondensed-bold", 7, Font.NORMAL, BaseColor.GREEN);

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

                    //INCIDENCIA
                    ColumnText incidenciaMes = new ColumnText(directContent);
                    string incidenciaText = ((int)(estabelecimento.ExtratoMesCompetencia.IncidenciaReal * 100)).ToString() + "%";
                    var incidenciaMesPhrase = new Phrase(new Chunk(incidenciaText, fontVendasBold)); 
                    incidenciaMes.SetSimpleColumn(incidenciaMesPhrase, 270, 560, 185, 595, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    incidenciaMes.Go();

                    //RECEITA NAO CAPTURADA
                    ColumnText receitaMes = new ColumnText(directContent);
                    string receitaMesText = (estabelecimento.ExtratoMesCompetencia.ReceitaNaoCapturada * -1).ToString("C2");
                    var receitaMesPhrase = new Phrase(new Chunk(receitaMesText, fontVendasReceita)); 
                    receitaMes.SetSimpleColumn(receitaMesPhrase, 380, 560, 290, 595, 25, Element.ALIGN_BOTTOM | Element.ALIGN_LEFT);
                    receitaMes.Go();

                    //MES COMPETENCIA
                    ColumnText mesCompetencia = new ColumnText(directContent);
                    string mesCompetenciaText = estabelecimento.Periodo();
                    var mesCompetenciaPhrase = new Phrase(new Chunk(mesCompetenciaText, fontMes)); 
                    mesCompetencia.SetSimpleColumn(mesCompetenciaPhrase, 350, 500, 45, 545, 25, Element.ALIGN_BOTTOM | Element.ALIGN_LEFT);
                    mesCompetencia.Go();

                    //RECEITA TOTAL NAO CAPTURADA
                    ColumnText receitaTotalMes = new ColumnText(directContent);
                    string receitaTotalMesText = (estabelecimento.ExtratoVendas.Sum(x=>x.ReceitaNaoCapturada) * -1).ToString("C2");
                    var receitaTotalMesPhrase = new Phrase(new Chunk(receitaTotalMesText, fontVendasReceita)); 
                    receitaTotalMes.SetSimpleColumn(receitaTotalMesPhrase, 380, 510, 290, 545, 25, Element.ALIGN_BOTTOM | Element.ALIGN_LEFT);
                    receitaTotalMes.Go();


                    // DADOS DA POSICAO
                    var posicaoTexto = $"{posicao.ToString()}º";
                    PdfContentByte cb = worker.DirectContent;
                    ColumnText ct = new ColumnText(cb);
                    var posicaoPhrase = new Phrase(new Chunk($"{posicao.ToString()}º", fontPosicaoRede)); 
                    ct.SetSimpleColumn(posicaoPhrase,  920, 100, 50, 580, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    ct.Go();

                    const int FATOR_FIXO = 31;

                    // VOLUME PEDIDO
                    int qtdExtrato = estabelecimento.ExtratoVendas.Count;
                    ColumnText[] volumePedido = new ColumnText[qtdExtrato];
                    string[] volumePedidoText = new string[qtdExtrato];
                    Phrase[] volumePedidoPhrase = new Phrase[qtdExtrato];

                    for (var index = 0; index < qtdExtrato; index++)
                    {
                        volumePedido[index] = new ColumnText(directContent);
                        volumePedidoText[index] = estabelecimento.ExtratoVendas[index].TotalPedidos.ToString("N0");
                        volumePedidoPhrase[index] = new Phrase(new Chunk(volumePedidoText[index], fontValoresGraf));
                        int fatorPosicao = (index * FATOR_FIXO);
                        volumePedido[index].SetSimpleColumn(volumePedidoPhrase[index], 285 + fatorPosicao, 382, 200 + fatorPosicao, 432, 25, Element.ALIGN_BOTTOM | Element.ALIGN_CENTER);
                        volumePedido[index].Go();    
                    }
                    
                    // VOLUME PEDIDO COM COCA
                    ColumnText[] volumePedidoCoca = new ColumnText[qtdExtrato];
                    string[] volumePedidoCocaText = new string[qtdExtrato];
                    Phrase[] volumePedidoCocaPhrase = new Phrase[qtdExtrato];

                    for (var index = 0; index < qtdExtrato; index++)
                    {
                        volumePedidoCoca[index] = new ColumnText(directContent);
                        volumePedidoCocaText[index] = estabelecimento.ExtratoVendas[index].PedidosComCocaCola.ToString("N0");
                        volumePedidoCocaPhrase[index] = new Phrase(new Chunk(volumePedidoCocaText[index], fontValoresGraf));
                        int fatorPosicao = (index * FATOR_FIXO);
                        volumePedidoCoca[index].SetSimpleColumn(volumePedidoCocaPhrase[index], 285 + fatorPosicao, 367, 200 + fatorPosicao, 417, 25, Element.ALIGN_BOTTOM | Element.ALIGN_CENTER);
                        volumePedidoCoca[index].Go();    
                    }

                    // MESES
                    ColumnText[] meses = new ColumnText[qtdExtrato];
                    string[] mesesText = new string[qtdExtrato];
                    Phrase[] mesesPhrase = new Phrase[qtdExtrato];

                    for (var index = 0; index < qtdExtrato; index++)
                    {
                        meses[index] = new ColumnText(directContent);
                        mesesText[index] = estabelecimento.ExtratoVendas[index].Competencia.ToString("MMM yy").PriMaiuscula();
                        mesesPhrase[index] = new Phrase(new Chunk(mesesText[index], fontValoresGraf));
                        int fatorPosicao = (index * FATOR_FIXO);
                        meses[index].SetSimpleColumn(mesesPhrase[index], 285 + fatorPosicao, 212, 200 + fatorPosicao, 272, 25, Element.ALIGN_BOTTOM | Element.ALIGN_CENTER);
                        meses[index].Go();    
                    }

                    // APROVEITAMENTO
                    ColumnText[] aproveitamento = new ColumnText[qtdExtrato];
                    string[] aproveitamentoText = new string[qtdExtrato];
                    Phrase[] aproveitamentoPhrase = new Phrase[qtdExtrato];

                    for (var index = 0; index < qtdExtrato; index++)
                    {
                        decimal aprovMeta = estabelecimento.ExtratoVendas[index].IncidenciaReal - estabelecimento.ExtratoVendas[index].Meta;
                        aproveitamento[index] = new ColumnText(directContent);
                        aproveitamentoText[index] = aprovMeta.ToString("P0");
                        aproveitamentoPhrase[index] = new Phrase(new Chunk(aproveitamentoText[index], fontValoresGraf));
                        int fatorPosicao = (index * FATOR_FIXO);
                        aproveitamento[index].SetSimpleColumn(aproveitamentoPhrase[index], 197 + fatorPosicao, 195, 217 + fatorPosicao, 252, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                        aproveitamento[index].Go();    
                    }

                    // PEDIDOS NAO CAPTURADOS
                    ColumnText[] naoCapiturados = new ColumnText[qtdExtrato];
                    string[] naoCapituradosText = new string[qtdExtrato];
                    Phrase[] naoCapituradosPhrase = new Phrase[qtdExtrato];

                    for (var index = 0; index < qtdExtrato; index++)
                    {
                        int qtde = estabelecimento.ExtratoVendas[index].TotalPedidosNaoCapturados * -1;
                        Font fontNaoCap = qtde < 0 ? fontValoresGrafRed : qtde == 0 ? fontValoresGraf : fontValoresGrafGreen;
                        naoCapiturados[index] = new ColumnText(directContent);
                        naoCapituradosText[index] = qtde.ToString("N0");
                        naoCapituradosPhrase[index] = new Phrase(new Chunk(naoCapituradosText[index], fontNaoCap));
                        int fatorPosicao = (index * FATOR_FIXO);
                        naoCapiturados[index].SetSimpleColumn(naoCapituradosPhrase[index], 197 + fatorPosicao, 173, 217 + fatorPosicao, 228, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                        naoCapiturados[index].Go();    
                    }

                    // PREÇO MÉDIO UNITÁRIO
                    ColumnText[] precoMedio = new ColumnText[qtdExtrato];
                    string[] precoMedioText = new string[qtdExtrato];
                    Phrase[] precoMedioPhrase = new Phrase[qtdExtrato];

                    for (var index = 0; index < qtdExtrato; index++)
                    {
                        precoMedio[index] = new ColumnText(directContent);
                        precoMedioText[index] = estabelecimento.ExtratoVendas[index].PrecoUnitarioMedio.ToString("C2");
                        precoMedioPhrase[index] = new Phrase(new Chunk(precoMedioText[index], fontValoresGraf));
                        int fatorPosicao = (index * FATOR_FIXO);
                        precoMedio[index].SetSimpleColumn(precoMedioPhrase[index], 197 + fatorPosicao, 173, 217 + fatorPosicao, 211, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                        precoMedio[index].Go();    
                    }

                    // CRIFRÃO FIXO
                    ColumnText[] crifaofixo = new ColumnText[qtdExtrato];
                    string[] crifaofixoText = new string[qtdExtrato];
                    Phrase[] crifaofixoPhrase = new Phrase[qtdExtrato];

                    for (var index = 0; index < qtdExtrato; index++)
                    {
                        decimal receita = estabelecimento.ExtratoVendas[index].ReceitaNaoCapturada * -1;
                        Font fontNaoCap = receita < 0 ? fontValoresGrafRed : receita == 0 ? fontValoresGraf : fontValoresGrafGreen;
                        crifaofixo[index] = new ColumnText(directContent);
                        crifaofixoText[index] = "R$";
                        crifaofixoPhrase[index] = new Phrase(new Chunk(crifaofixoText[index], fontNaoCap));
                        int fatorPosicao = (index * FATOR_FIXO);
                        crifaofixo[index].SetSimpleColumn(crifaofixoPhrase[index], 203 + fatorPosicao, 167, 213 + fatorPosicao, 197, 25, Element.ALIGN_TOP | Element.ALIGN_CENTER);
                        crifaofixo[index].Go();    
                    }

                    // RECEITA NAO CAPTURADOS
                    ColumnText[] receitaNaoCapiturados = new ColumnText[qtdExtrato];
                    string[] receitaNaoCapituradosText = new string[qtdExtrato];
                    Phrase[] receitaNaoCapituradosPhrase = new Phrase[qtdExtrato];

                    for (var index = 0; index < qtdExtrato; index++)
                    {
                        decimal receita = estabelecimento.ExtratoVendas[index].ReceitaNaoCapturada * -1;
                        Font fontNaoCap = receita < 0 ? fontValoresGrafRed : receita == 0 ? fontValoresGraf : fontValoresGrafGreen;
                        receitaNaoCapiturados[index] = new ColumnText(directContent);
                        receitaNaoCapituradosText[index] = receita.ToString("N2");
                        receitaNaoCapituradosPhrase[index] = new Phrase(new Chunk(receitaNaoCapituradosText[index], fontNaoCap));
                        int fatorPosicao = (index * FATOR_FIXO);
                        receitaNaoCapiturados[index].SetSimpleColumn(receitaNaoCapituradosPhrase[index], 197 + fatorPosicao, 157, 227 + fatorPosicao, 187, 25, Element.ALIGN_TOP | Element.ALIGN_CENTER);
                        receitaNaoCapiturados[index].Go();    
                    }

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