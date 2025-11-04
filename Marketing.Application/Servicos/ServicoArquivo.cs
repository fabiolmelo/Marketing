using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using Microsoft.AspNetCore.Http;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Marketing.Domain.Extensoes;
using Marketing.Domain.Interfaces.IUnitOfWork;

namespace Marketing.Application.Servicos
{
    public class ServicoArquivo : Servico<ImportacaoEfetuada>, IServicoArquivos
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServicoArquivo(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> UploadArquivo(IFormFile arquivo)
        {
            if (arquivo == null) return null;
            var arquivoImportado = $"{Guid.NewGuid().ToString()}.xlsx";
            var filePath = Path.Combine("DadosApp", "Planilhas", arquivoImportado);
            try
            {
                using (FileStream filestream = System.IO.File.Create(filePath))
                {
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
                                      int posicao, String contentRootPath,
                                      string caminhoApp)
        {
            var caminhoFundo = Path.Combine(contentRootPath, "DadosApp", "fundo_export.png");
            var caminhoFontes = Path.Combine(contentRootPath, "DadosApp", "Fonts");
            //var caminhoFundo = Path.Combine(contentRootPath, "DadosApp", "CocaColaFundo.jpeg");
            var caminhoPdf = Path.Combine(contentRootPath, "DadosApp", "tmp.pdf");
            var caminhoPdfCompleto = Path.Combine(contentRootPath, "DadosApp", "images", $"{arquivoPdf}");
            var caminhoPdfPage2 = Path.Combine(contentRootPath, "DadosApp", "Entenda seu extrato Coca-Cola_v3.pdf");
            var caminhoGrafico = Path.Combine(contentRootPath, "DadosApp", "Grafico.jpg");
            var caminhoSetaMeta = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaMeta.png");
            var caminhoSetaIncidencia5 = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaIncidencia5.png");
            var caminhoSetaIncidencia20 = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaIncidencia20.png");
            var caminhoSetaIncidencia40 = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaIncidencia40.png");
            var caminhoSetaIncidencia50 = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaIncidencia50.png");
            var caminhoSetaIncidencia60 = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaIncidencia60.png");
            var caminhoSetaIncidencia80 = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaIncidencia80.png");
            var caminhoSetaIncidencia95 = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaIncidencia95.png");

            using (var image = File.OpenRead(caminhoFundo))
            {
                if (File.Exists(caminhoPdf)) File.Delete(caminhoPdf);
                using (FileStream filestream = new FileStream(caminhoPdf, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    Document document = new Document(PageSize.A4);
                    var worker = PdfWriter.GetInstance(document, filestream);
                    document.Open();

                    //REGISTRAR FONTES 
                    FontFactory.RegisterDirectory(caminhoFontes);
                    List<string> fontesRegistradas = FontFactory.RegisteredFonts.ToList();


                    // FONTES
                    var fontes = FontFactory.RegisteredFonts;
                    Font fontDadosEstabelecimento = FontFactory.GetFont("tccc unity", BaseFont.CP1252, BaseFont.EMBEDDED, 8, Font.NORMAL, BaseColor.WHITE);
                    Font fontPosicaoRede = FontFactory.GetFont("tccc-unityheadline-bold", BaseFont.CP1252, BaseFont.EMBEDDED, 28, Font.NORMAL, BaseColor.WHITE);
                    Font fontMesReferencia = FontFactory.GetFont("tccc-unityheadline-bold", BaseFont.CP1252, BaseFont.EMBEDDED, 11, Font.NORMAL, BaseColor.BLACK);

                    Font fontVendas = FontFactory.GetFont("tccc unity", BaseFont.CP1252, BaseFont.EMBEDDED, 12, Font.NORMAL, BaseColor.BLACK);
                    Font fontVendasBold = FontFactory.GetFont("tccc-unitytext bold", BaseFont.CP1252, BaseFont.EMBEDDED, 12, Font.NORMAL, BaseColor.BLACK);
                    Font fontVendasReceitaMes = FontFactory.GetFont("tccc-unitytext bold", BaseFont.CP1252, BaseFont.EMBEDDED, 12, Font.NORMAL, ((estabelecimento.ExtratoMesCompetencia.ReceitaNaoCapturada * -1) < 0) ? BaseColor.RED : new BaseColor(13, 163, 13));
                    Font fontVendasReceitaTotal = FontFactory.GetFont("tccc-unitytext bold", BaseFont.CP1252, BaseFont.EMBEDDED, 12, Font.NORMAL, ((estabelecimento.ExtratoVendas.Sum(x => x.ReceitaNaoCapturada) * -1)) < 0 ? BaseColor.RED : new BaseColor(13, 163, 13));
                    Font fontMes = FontFactory.GetFont("tcccunity-bold", BaseFont.CP1252, BaseFont.EMBEDDED, 9, Font.NORMAL, BaseColor.BLACK);
                    Font fontValoresGraf = FontFactory.GetFont("tccc-unitycondensed-bold", BaseFont.CP1252, BaseFont.EMBEDDED, 7, Font.NORMAL, BaseColor.BLACK);
                    Font fontValoresGrafRed = FontFactory.GetFont("tccc-unitycondensed-bold", BaseFont.CP1252, BaseFont.EMBEDDED, 7, Font.NORMAL, BaseColor.RED);
                    Font fontValoresGrafGreen = FontFactory.GetFont("tccc-unitycondensed-bold", BaseFont.CP1252, BaseFont.EMBEDDED, 7, Font.NORMAL, new BaseColor(13, 163, 13));
                    Font fontValoresIncidencia = FontFactory.GetFont("tccc-unitycondensed-bold", BaseFont.CP1252, BaseFont.EMBEDDED, 9, Font.NORMAL, BaseColor.WHITE);

                    Font fontTextoIncidencia = FontFactory.GetFont("tccc-unityheadline-bold", BaseFont.CP1252, BaseFont.EMBEDDED, 7, Font.NORMAL, BaseColor.GRAY);
                    Font fontTextoMeta = FontFactory.GetFont("tccc-unityheadline-bold", BaseFont.CP1252, BaseFont.EMBEDDED, 7, Font.NORMAL, BaseColor.RED);

                    //GRAVA O FUNDO NO ARQUIVO
                    var pic = iTextSharp.text.Image.GetInstance(caminhoFundo);
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
                    columnText4.SetSimpleColumn(posicaoDados4, 650, 100, 50, 670, 25, Element.ALIGN_LEFT | Element.ALIGN_LEFT);
                    columnText4.Go();

                    //MES REFERENCIA
                    string textoMesReferencia = $"{estabelecimento.MesCompetencia} (META DE INCIDÊNCIA: ";
                    textoMesReferencia += $"{(int)(estabelecimento.ExtratoMesCompetencia.Meta * 100)}%)";
                    ColumnText mesReferencia = new ColumnText(directContent);
                    var mesReferenciaPhrase = new Phrase(new Chunk(textoMesReferencia, fontMesReferencia));
                    mesReferencia.SetSimpleColumn(mesReferenciaPhrase, 600, 550, 50, 635, 25, Element.ALIGN_LEFT | Element.ALIGN_LEFT);
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
                    var receitaMesPhrase = new Phrase(new Chunk(receitaMesText, fontVendasReceitaMes));
                    receitaMes.SetSimpleColumn(receitaMesPhrase, 500, 560, 290, 595, 25, Element.ALIGN_BOTTOM | Element.ALIGN_LEFT);
                    receitaMes.Go();

                    //MES COMPETENCIA
                    ColumnText mesCompetencia = new ColumnText(directContent);
                    string mesCompetenciaText = estabelecimento.Periodo();
                    var mesCompetenciaPhrase = new Phrase(new Chunk(mesCompetenciaText, fontMes));
                    mesCompetencia.SetSimpleColumn(mesCompetenciaPhrase, 350, 500, 45, 545, 25, Element.ALIGN_BOTTOM | Element.ALIGN_LEFT);
                    mesCompetencia.Go();

                    //RECEITA TOTAL NAO CAPTURADA
                    ColumnText receitaTotalMes = new ColumnText(directContent);
                    string receitaTotalMesText = (estabelecimento.ExtratoVendas.Sum(x => x.ReceitaNaoCapturada) * -1).ToString("C2");
                    var receitaTotalMesPhrase = new Phrase(new Chunk(receitaTotalMesText, fontVendasReceitaTotal));
                    receitaTotalMes.SetSimpleColumn(receitaTotalMesPhrase, 500, 510, 290, 545, 25, Element.ALIGN_BOTTOM | Element.ALIGN_LEFT);
                    receitaTotalMes.Go();


                    // DADOS DA POSICAO
                    var posicaoTexto = $"{posicao.ToString()}º";
                    PdfContentByte cb = worker.DirectContent;
                    ColumnText ct = new ColumnText(cb);
                    var posicaoPhrase = new Phrase(new Chunk($"{posicao.ToString()}º", fontPosicaoRede));
                    ct.SetSimpleColumn(posicaoPhrase, 920, 100, 50, 580, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
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
                        volumePedidoText[index] = estabelecimento.ExtratoVendas.ElementAt(index).TotalPedidos.ToString("N0");
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
                        volumePedidoCocaText[index] = estabelecimento.ExtratoVendas.ElementAt(index).PedidosComCocaCola.ToString("N0");
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
                        mesesText[index] = estabelecimento.ExtratoVendas.ElementAt(index).Competencia.ToString("MMM yy").PriMaiuscula();
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
                        decimal aprovMeta = estabelecimento.ExtratoVendas.ElementAt(index).IncidenciaReal - estabelecimento.ExtratoVendas.ElementAt(index).Meta;
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
                        int qtde = estabelecimento.ExtratoVendas.ElementAt(index).TotalPedidosNaoCapturados * -1;
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
                        precoMedioText[index] = estabelecimento.ExtratoVendas.ElementAt(index).PrecoUnitarioMedio.ToString("C2");
                        precoMedioPhrase[index] = new Phrase(new Chunk(precoMedioText[index], fontValoresGraf));
                        int fatorPosicao = (index * FATOR_FIXO);
                        precoMedio[index].SetSimpleColumn(precoMedioPhrase[index], 197 + fatorPosicao, 173, 217 + fatorPosicao, 211, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                        precoMedio[index].Go();
                    }

                    // CIFRÃO FIXO
                    ColumnText[] crifaofixo = new ColumnText[qtdExtrato];
                    string[] crifaofixoText = new string[qtdExtrato];
                    Phrase[] crifaofixoPhrase = new Phrase[qtdExtrato];

                    for (var index = 0; index < qtdExtrato; index++)
                    {
                        decimal receita = estabelecimento.ExtratoVendas.ElementAt(index).ReceitaNaoCapturada * -1;
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
                        decimal receita = estabelecimento.ExtratoVendas.ElementAt(index).ReceitaNaoCapturada * -1;
                        Font fontNaoCap = receita < 0 ? fontValoresGrafRed : receita == 0 ? fontValoresGraf : fontValoresGrafGreen;
                        receitaNaoCapiturados[index] = new ColumnText(directContent);
                        receitaNaoCapituradosText[index] = receita.ToString("N2");
                        receitaNaoCapituradosPhrase[index] = new Phrase(new Chunk(receitaNaoCapituradosText[index], fontNaoCap));
                        int fatorPosicao = (index * FATOR_FIXO);
                        receitaNaoCapiturados[index].SetSimpleColumn(receitaNaoCapituradosPhrase[index], 197 + fatorPosicao, 157, 227 + fatorPosicao, 187, 25, Element.ALIGN_TOP | Element.ALIGN_CENTER);
                        receitaNaoCapiturados[index].Go();
                    }

                    //PLOTAR A IMAGEM DO GRAFICO
                    var graficoImage = iTextSharp.text.Image.GetInstance(caminhoGrafico);
                    graficoImage.SetAbsolutePosition(190, 259);
                    graficoImage.ScaleAbsoluteHeight(125);
                    graficoImage.ScaleAbsoluteWidth(380);
                    document.Add(graficoImage);

                    //PLOTAR A IMAGEM DA SETA META
                    iTextSharp.text.Image setaMeta = iTextSharp.text.Image.GetInstance(caminhoSetaMeta);
                    setaMeta.SetAbsolutePosition(425, 430);
                    document.Add(setaMeta);

                    //PLOTAR A IMAGEM DA SETA INCIDENCIA
                    iTextSharp.text.Image setaIncidencia;

                    if (estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.1) < 0)
                    {
                        setaIncidencia = iTextSharp.text.Image.GetInstance(caminhoSetaIncidencia5);
                    }
                    else if (estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.1) >= 0 &&
                             estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.35) < 0)
                    {
                        setaIncidencia = iTextSharp.text.Image.GetInstance(caminhoSetaIncidencia20);
                    }
                    else if (estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.35) >= 0 &&
                             estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.45) < 0)
                    {
                        setaIncidencia = iTextSharp.text.Image.GetInstance(caminhoSetaIncidencia40);
                    }
                    else if (estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.45) >= 0 &&
                             estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.55) < 0)
                    {
                        setaIncidencia = iTextSharp.text.Image.GetInstance(caminhoSetaIncidencia50);
                    }
                    else if (estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.55) >= 0 &&
                             estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.75) < 0)
                    {
                        setaIncidencia = iTextSharp.text.Image.GetInstance(caminhoSetaIncidencia60);
                    }
                    else if (estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.75) >= 0 &&
                             estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.95) < 0)
                    {
                        setaIncidencia = iTextSharp.text.Image.GetInstance(caminhoSetaIncidencia80);
                    }
                    else
                    {
                        setaIncidencia = iTextSharp.text.Image.GetInstance(caminhoSetaIncidencia95);
                    }
                    
                    setaIncidencia.SetAbsolutePosition(275, 430);
                    document.Add(setaIncidencia);

                    // DESENHAR A LINHA TRACEJADA DA META
                    float posicaoMetaY = 260 + (float)(estabelecimento.ExtratoMesCompetencia.Meta * 110);
                    float posicaoIncidenciaY = 260 + (float)(estabelecimento.IncidenciaMedia * 110);
                    float posicaoTextoMeta = (float)(estabelecimento.ExtratoMesCompetencia.Meta >=
                                              estabelecimento.ExtratoMesCompetencia.IncidenciaReal ?
                                              posicaoMetaY + 5 : posicaoMetaY - 5);
                    float posicaoTextoIncidencia = (float)(estabelecimento.ExtratoMesCompetencia.IncidenciaReal >=
                                              estabelecimento.ExtratoMesCompetencia.Meta ?
                                              posicaoIncidenciaY + 5 : posicaoIncidenciaY - 5);

                    cb.SetLineDash(4.5f, 4.5f);
                    cb.SetRGBColorStroke(237, 34, 36);
                    cb.SetLineWidth(0.5f);
                    cb.MoveTo(150, posicaoMetaY);
                    cb.LineTo(565, posicaoMetaY);
                    cb.Stroke();
                    cb.SetColorStroke(BaseColor.BLACK);

                    // DESENHAR A LINHA TRACEJADA DA INCIDENCIA
                    cb.SetLineDash(4.5f, 4.5f);
                    cb.SetColorStroke(BaseColor.GRAY);
                    //cb.MoveTo(100, 260);                    
                    //cb.LineTo(565, 260);
                    cb.MoveTo(150, posicaoIncidenciaY);
                    cb.LineTo(565, posicaoIncidenciaY);
                    cb.Stroke();
                    cb.SetColorStroke(BaseColor.BLACK);


                    // PALAVRA META NO GRAFICO
                    ColumnText textMeta = new ColumnText(directContent);
                    var textMetaPhrase = new Phrase(new Chunk("META", fontTextoMeta));
                    textMeta.SetSimpleColumn(textMetaPhrase, 150, posicaoTextoMeta, 100,
                                             posicaoTextoMeta, 0, Element.ALIGN_RIGHT);
                    textMeta.Go();

                    // PALAVRA INCIDENCIA NO GRAFICO
                    ColumnText textIncidencia = new ColumnText(directContent);
                    var textIncidenciaPhrase = new Phrase(new Chunk("INCIDÊNCIA REAL", fontTextoIncidencia));
                    textIncidencia.SetSimpleColumn(textIncidenciaPhrase, 150, posicaoTextoIncidencia - 5, 40,
                                                   posicaoTextoIncidencia - 5, 0, Element.ALIGN_RIGHT);
                    textIncidencia.Go();


                    // INCIDENCIA GRAFICO
                    ColumnText[] incidenciaReal = new ColumnText[qtdExtrato];
                    string[] incidenciaRealText = new string[qtdExtrato];
                    Phrase[] incidenciaRealPhrase = new Phrase[qtdExtrato];

                    for (var index = 0; index < qtdExtrato; index++)
                    {
                        int incidenciaRealValor = (int)(estabelecimento.ExtratoVendas.ElementAt(index).IncidenciaReal * 100);
                        incidenciaReal[index] = new ColumnText(directContent);
                        incidenciaRealText[index] = $"{incidenciaRealValor.ToString("N0")}%";
                        incidenciaRealPhrase[index] = new Phrase(new Chunk(incidenciaRealText[index], fontValoresIncidencia));
                    }

                    if (qtdExtrato > 0) incidenciaReal[0].SetSimpleColumn(incidenciaRealPhrase[0], 200, 265, 218, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    if (qtdExtrato > 0) incidenciaReal[0].Go();
                    if (qtdExtrato > 1) incidenciaReal[1].SetSimpleColumn(incidenciaRealPhrase[1], 230, 265, 250, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    if (qtdExtrato > 1) incidenciaReal[1].Go();
                    if (qtdExtrato > 2) incidenciaReal[2].SetSimpleColumn(incidenciaRealPhrase[2], 261, 265, 281, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    if (qtdExtrato > 2) incidenciaReal[2].Go();
                    if (qtdExtrato > 3) incidenciaReal[3].SetSimpleColumn(incidenciaRealPhrase[3], 290, 265, 310, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    if (qtdExtrato > 3) incidenciaReal[3].Go();
                    if (qtdExtrato > 4) incidenciaReal[4].SetSimpleColumn(incidenciaRealPhrase[4], 322, 265, 342, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    if (qtdExtrato > 4) incidenciaReal[4].Go();
                    if (qtdExtrato > 5) incidenciaReal[5].SetSimpleColumn(incidenciaRealPhrase[5], 354, 265, 372, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    if (qtdExtrato > 5) incidenciaReal[5].Go();
                    if (qtdExtrato > 6) incidenciaReal[6].SetSimpleColumn(incidenciaRealPhrase[6], 382, 265, 402, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    if (qtdExtrato > 6) incidenciaReal[6].Go();
                    if (qtdExtrato > 7) incidenciaReal[7].SetSimpleColumn(incidenciaRealPhrase[7], 414, 265, 432, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    if (qtdExtrato > 7) incidenciaReal[7].Go();
                    if (qtdExtrato > 8) incidenciaReal[8].SetSimpleColumn(incidenciaRealPhrase[8], 446, 265, 464, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    if (qtdExtrato > 8) incidenciaReal[8].Go();
                    if (qtdExtrato > 9) incidenciaReal[9].SetSimpleColumn(incidenciaRealPhrase[9], 476, 265, 494, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    if (qtdExtrato > 9) incidenciaReal[9].Go();
                    if (qtdExtrato > 10) incidenciaReal[10].SetSimpleColumn(incidenciaRealPhrase[10], 506, 265, 524, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    if (qtdExtrato > 10) incidenciaReal[10].Go();
                    if (qtdExtrato > 11) incidenciaReal[11].SetSimpleColumn(incidenciaRealPhrase[11], 536, 265, 555, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    if (qtdExtrato > 11) incidenciaReal[11].Go();

                    filestream.Flush();
                    document.CloseDocument();

                    // PAGINA 2 -TEXTO EXPLICATIVO
                    Document PDFdoc = new Document();
                    using (FileStream MyFileStream = new(caminhoPdfCompleto, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        PdfCopy PDFwriter = new PdfCopy(PDFdoc, MyFileStream);
                        if (PDFwriter == null) return "";
                        PDFdoc.Open();

                        PdfReader PDFreader = new PdfReader(caminhoPdf);
                        PDFreader.ConsolidateNamedDestinations();
                        PdfImportedPage page = PDFwriter.GetImportedPage(PDFreader, 1);
                        PDFwriter.AddPage(page);

                        PdfReader PDFreader2 = new PdfReader(caminhoPdfPage2);
                        PDFreader2.ConsolidateNamedDestinations();
                        PdfImportedPage page2 = PDFwriter.GetImportedPage(PDFreader2, 1);
                        PDFwriter.AddPage(page2);

                        PDFreader.Close();
                        PDFreader2.Close();
                        PDFdoc.CloseDocument();
                    }
                }
            }
            return arquivoPdf; 
        }

        public List<DadosPlanilha> LerDados(string pathArquivo, string rede)
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
                        var fone = linha.Cell("N").Value.ToString().RemoverCaracteresEspeciais();
                        if (fone.Length == 10 || fone.Length == 11) fone = $"55{fone}";

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

        public void GerarRelatorioEnvios(string pathArquivo, List<Mensagem> mensagens,
                                         List<ResumoMensagem> resumo, string pathArquivoBase)
        {
            if (System.IO.File.Exists(pathArquivoBase)) File.Delete(pathArquivoBase);
            using(var excel = new ClosedXML.Excel.XLWorkbook(pathArquivo))
            {
                var linha = 7;
                var planilha = excel.Worksheet(1);
                foreach (var mensagem in mensagens)
                {
                    var ultimoStatus = mensagem.MensagemItems.OrderByDescending(x => x.DataEvento).FirstOrDefault();
                    planilha.Range($"A{linha}:H{linha}").Style.Font.FontSize = 10;  
                    planilha.Cell($"A{linha}").Value = mensagem.EnvioMensagemMensal?.DataGeracao.ToShortDateString() ?? "";
                    planilha.Cell($"B{linha}").Value = mensagem.EnvioMensagemMensal?.RedeNome ?? "";
                    planilha.Cell($"C{linha}").Value = mensagem.EnvioMensagemMensal?.NomeFranquia ?? "";
                    planilha.Cell($"D{linha}").Value = mensagem.EnvioMensagemMensal?.EstabelecimentoCnpj ?? "";
                    planilha.Cell($"E{linha}").Value = mensagem.EnvioMensagemMensal?.ContatoTelefone ?? "";
                    planilha.Cell($"F{linha}").Value = ultimoStatus?.MensagemStatus.ToString() ?? "";
                    planilha.Cell($"G{linha}").Value = ultimoStatus?.Observacao?.ToString() ?? "";
                    planilha.Row(linha + 1).AdjustToContents(); 
                    planilha.Row(linha+1).InsertRowsBelow(1);
                    linha++;
                }

                planilha.Cell($"B{linha + 5}").Value = resumo.FirstOrDefault(x => x.MensagemStatus == MensagemStatus.Entregue)?.Qtd ?? 0;
                planilha.Cell($"B{linha + 6}").Value = resumo.FirstOrDefault(x => x.MensagemStatus == MensagemStatus.Lida)?.Qtd ?? 0;
                planilha.Cell($"B{linha + 7}").Value = resumo.FirstOrDefault(x => x.MensagemStatus == MensagemStatus.ClicouLink)?.Qtd ?? 0;
                planilha.Cell($"B{linha + 8}").Value = resumo.FirstOrDefault(x => x.MensagemStatus == MensagemStatus.Disparado ||
                                                                             x.MensagemStatus == MensagemStatus.Falha)?.Qtd ?? 0;
                excel.SaveAs(pathArquivoBase);
            }
        }

        public async Task<bool> AtualizarContatoViaPlanilhaEmailMarketing(string pathArquivo)
        {
            int row = 2;
            var dadosPlanilha = new List<DadosPlanilha>();

            try
            {
                using (var excel = new ClosedXML.Excel.XLWorkbook(pathArquivo))
                {
                    var planilha = excel.Worksheet(1).RowsUsed();
                    foreach (var linha in planilha)
                    {
                        if (linha.RowNumber() > 1 && !linha.IsEmpty())
                        {
                            var dataCadastro = linha.Cell("A").Value.GetDateTime();
                            var fone = linha.Cell("B") == null ? "" : linha.Cell("B").Value.ToString().RemoverCaracteresEspeciais();
                            if (fone.Length == 11) fone = $"55{fone}";
                            var nome = linha.Cell("C") == null ? "" : linha.Cell("C").Value.ToString().ToUpper();
                            var contato = await _unitOfWork.repositorioContato.BuscarContatosIncludeEstabelecimento(fone);
                            string cnpj;
                            if (contato == null)
                            {
                                contato = new Contato()
                                {
                                    Telefone = fone,
                                    Nome = nome,
                                    DataCadastro = dataCadastro,
                                    OrigemContato = OrigemContato.EmailMarketing

                                };
                                await _unitOfWork.repositorioContato.AddAsync(contato);
                                await _unitOfWork.CommitAsync();
                            }
                            for(int index = 4; index<=31; index+=3)
                            {
                                cnpj = linha.Cell(index).Value.ToString();
                                var estabelecimento = await _unitOfWork.repositorioEstabelecimento.GetByIdStringAsync(cnpj);
                                if(estabelecimento != null)
                                {
                                    var contatoEstabelecimento = await _unitOfWork.GetRepository<ContatoEstabelecimento>()
                                                                .GetByIdChaveComposta(fone, cnpj);
                                    if (contatoEstabelecimento == null)
                                    {
                                        contatoEstabelecimento = new ContatoEstabelecimento(fone, cnpj);
                                        await _unitOfWork.GetRepository<ContatoEstabelecimento>().AddAsync(contatoEstabelecimento);
                                        await _unitOfWork.CommitAsync();
                                    }
                                }
                            }
                        }
                        row++;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na linha: {row}.\n{ex.Message}");
                return false;
            }     
        }
    }
}