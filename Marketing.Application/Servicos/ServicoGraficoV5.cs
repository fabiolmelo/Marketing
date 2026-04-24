using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Marketing.Domain.Extensoes;

namespace Marketing.Application.Servicos
{
    public class ServicoGraficoV5 : IServicoGraficoRevisado
    {
        public string GerarArquivoPdf(Estabelecimento estabelecimento, string arquivoPdf, int posicao, string contentRootPath, string caminhoApp)
        {
            var caminhoFundo = Path.Combine(contentRootPath, "DadosApp", "FundoCocaV4_1.png");
            var caminhoFontes = Path.Combine(contentRootPath, "DadosApp", "Fonts");
            var caminhoPdf = Path.Combine(contentRootPath, "DadosApp", "tmp.pdf");
            var caminhoPdfCompleto = Path.Combine(contentRootPath, "DadosApp", "images", $"{arquivoPdf}");
            var caminhoPdfPage2 = Path.Combine(contentRootPath, "DadosApp", "Entenda seu extrato Coca-Cola_v4.pdf");
            var caminhoGrafico = Path.Combine(contentRootPath, "DadosApp", "Grafico.png");
            var caminhoSetaMeta = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaMeta.png");
            var caminhoSetaIncidencia5 = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaIncidencia5.png");
            var caminhoSetaIncidencia20 = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaIncidencia20.png");
            var caminhoSetaIncidencia40 = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaIncidencia40.png");
            var caminhoSetaIncidencia50 = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaIncidencia50.png");
            var caminhoSetaIncidencia60 = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaIncidencia60.png");
            var caminhoSetaIncidencia80 = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaIncidencia80.png");
            var caminhoSetaIncidencia95 = Path.Combine(contentRootPath, "DadosApp", "Seta", "SetaIncidencia95.png");
            var caminhoLogoRede = Path.Combine(contentRootPath, "DadosApp", "Logos", "LogoTmp.png");

            // -------------------------------------------------------------------------
            // MAPEAMENTO DE FONTES (conforme PDF original analisado)
            //
            // Fontes presentes no PDF original:
            //   TCCCUnity-Regular      → family="TCCC Unity"    fullname="TCCC Unity Regular"
            //   TCCCUnity-Bold         → family="TCCC Unity"    fullname="TCCC Unity Bold"
            //   TCCCUnity-Black        → family="TCCC Unity Black" fullname="TCCC Unity Black"
            //   TCCC-UnityCondensed-Medium → family="TCCC-UnityCondensed" fullname="TCCC-UnityCondensed Medium"
            //   TCCC-UnityCondensed-Bold   → family="TCCC-UnityCondensed" fullname="TCCC-UnityCondensed Bold"
            //
            // Fontes NÃO presentes no PDF original (e portanto NÃO usadas nesta versão):
            //   TCCC-UnityHeadline-Bold  (arquivo disponível mas não usado no original)
            //   TCCC-UnityText-Bold      (arquivo disponível mas não usado no original)
            //
            // Nomes de registro no iTextSharp (após FontFactory.RegisterDirectory):
            //   "tccc unity"                  → TCCCUnity-Regular
            //   "tccc unity bold"             → TCCCUnity-Bold  (família "TCCC Unity", estilo Bold)
            //   "tccc unity black"            → TCCCUnity-Black
            //   "tccc-unitycondensed"         → TCCC-UnityCondensed-Medium (família "TCCC-UnityCondensed")
            //   "tccc-unitycondensed-bold"    → TCCC-UnityCondensed-Bold
            // -------------------------------------------------------------------------

            using (var image = File.OpenRead(caminhoFundo))
            {
                if (File.Exists(caminhoPdf)) File.Delete(caminhoPdf);
                if (File.Exists(caminhoPdfCompleto)) File.Delete(caminhoPdfCompleto);
                using (FileStream filestream = new FileStream(caminhoPdf, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    Document document = new Document(PageSize.A4);
                    var worker = PdfWriter.GetInstance(document, filestream);
                    document.Open();

                    // REGISTRAR FONTES
                    FontFactory.RegisterDirectory(caminhoFontes);
                    List<string> fontesRegistradas = FontFactory.RegisteredFonts.ToList();

                    // ---------------------------------------------------------------
                    // CORES (extraídas do PDF original via análise de spans)
                    // ---------------------------------------------------------------
                    // Texto geral / labels escuros:  RGB(35,31,32)  = #231F20
                    // Texto branco (sobre fundo):    RGB(255,255,255)
                    // Vermelho (receita negativa):   RGB(237,34,36)  = #ED2224
                    // Laranja-vermelho (ped. não cap.): RGB(254,1,0) = #FE0100  ← cor real do original
                    // Verde (valores positivos):     RGB(13,163,13)  = #0DA30D
                    // Roxo (Sua Incidência %):       RGB(146,39,143) = #92278F
                    // Verde-meta (Meta Incidência %): RGB(0,176,76)  = #00B04C
                    // ---------------------------------------------------------------

                    var corTextoGeral   = new BaseColor(35, 31, 32);       // #231F20 - labels/dados tabela
                    var corBranco       = BaseColor.WHITE;
                    var corVermelho     = new BaseColor(237, 34, 36);       // #ED2224 - receita negativa
                    var corLaranja      = new BaseColor(254, 1, 0);         // #FE0100 - pedidos não capturados negativos
                    var corVerde        = new BaseColor(13, 163, 13);       // #0DA30D - valores positivos
                    var corRoxo         = new BaseColor(146, 39, 143);      // #92278F - Sua Incidência %
                    var corVerdeMeta    = new BaseColor(0, 176, 76);        // #00B04C - Meta Incidência %

                    // ---------------------------------------------------------------
                    // FONTES — nomes exatos conforme análise do PDF original
                    // ---------------------------------------------------------------

                    // Dados do estabelecimento (Loja/Cidade/Endereço): TCCCUnity-Regular, 8pt, branco
                    Font fontDadosEstabelecimento = FontFactory.GetFont("tccc unity", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 8, Font.NORMAL, corBranco);

                    // Posição no ranking: TCCCUnity-Black, ~21pt, branco
                    Font fontPosicaoRede = FontFactory.GetFont("tccc unity black", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 21, Font.NORMAL, corBranco);

                    // Mês de referência (NOVEMBRO/2025...): TCCCUnity-Bold, 12pt, corTextoGeral
                    Font fontMesReferencia = FontFactory.GetFont("tccc unity bold", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12, Font.NORMAL, corTextoGeral);

                    // Valores numéricos do mês (TotalPedidos, PedidosComCoca): TCCCUnity-Regular, 12pt
                    Font fontVendas = FontFactory.GetFont("tccc unity", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12, Font.NORMAL, corTextoGeral);

                    // Incidência % do mês (50%): TCCCUnity-Bold, 12pt
                    Font fontVendasBold = FontFactory.GetFont("tccc unity bold", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12, Font.NORMAL, corTextoGeral);

                    // Receita não capturada do mês: TCCCUnity-Bold, 12pt, cor variável (vermelho/verde)
                    Font fontVendasReceitaMes = FontFactory.GetFont("tccc unity bold", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12, Font.NORMAL,
                        ((estabelecimento.ExtratoMesCompetencia.ReceitaNaoCapturada * -1) < 0) ? corVermelho : corVerde);

                    // Receita total acumulada: TCCCUnity-Bold, 12pt, cor variável
                    Font fontVendasReceitaTotal = FontFactory.GetFont("tccc unity bold", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12, Font.NORMAL,
                        ((estabelecimento.ExtratoVendas.Sum(x => x.ReceitaNaoCapturada) * -1)) < 0 ? corVermelho : corVerde);

                    // Meses na tabela (Jan/25 etc.): TCCC-UnityCondensed-Bold, 7pt (era "tcccunity-bold" → CORRIGIDO)
                    Font fontMes = FontFactory.GetFont("tccc-unitycondensed-bold", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 7, Font.NORMAL, corTextoGeral);

                    // Valores tabela geral: TCCC-UnityCondensed-Medium, 7pt (era "tccc-unitycondensed" sem especificar peso → Medium)
                    Font fontValoresGraf = FontFactory.GetFont("tccc-unitycondensed", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 7, Font.NORMAL, corTextoGeral);

                    // Receita não capturada negativa na tabela: TCCC-UnityCondensed-Medium, 7pt, vermelho (#ED2224)
                    Font fontValoresGrafRed = FontFactory.GetFont("tccc-unitycondensed", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 7, Font.NORMAL, corVermelho);

                    // Valores positivos na tabela: TCCC-UnityCondensed-Medium, 7pt, verde
                    Font fontValoresGrafGreen = FontFactory.GetFont("tccc-unitycondensed", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 7, Font.NORMAL, corVerde);

                    // Pedidos não capturados negativos: TCCC-UnityCondensed-Medium, 7pt, laranja-vermelho (#FE0100)
                    // Nota: o original usa #FE0100 para o campo "NÚMERO DE PEDIDOS SEM BEBIDAS" negativo
                    Font fontValoresGrafLaranja = FontFactory.GetFont("tccc-unitycondensed", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 7, Font.NORMAL, corLaranja);

                    // Valor da Meta no gauge: TCCCUnity-Bold, ~11pt, verde-meta #00B04C
                    Font fontCorMeta = FontFactory.GetFont("tccc unity bold", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 11, Font.NORMAL, corVerdeMeta);

                    // Valor da Incidência no gauge: TCCCUnity-Bold, ~11pt, roxo #92278F
                    Font fontCorIncidencia = FontFactory.GetFont("tccc unity bold", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 11, Font.NORMAL, corRoxo);

                    // % dentro das barras do gráfico: TCCCUnity-Bold, 6pt, branco
                    Font fontValoresIncidencia = FontFactory.GetFont("tccc unity bold", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 6, Font.NORMAL, corBranco);

                    // Label "INCIDÊNCIA REAL" no gráfico: TCCCUnity-Bold, 8pt, corTextoGeral
                    Font fontTextoIncidencia = FontFactory.GetFont("tccc unity bold", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 8, Font.NORMAL, corTextoGeral);

                    // Label "META" no gráfico: TCCCUnity-Bold, 8pt, vermelho
                    Font fontTextoMeta = FontFactory.GetFont("tccc unity bold", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 8, Font.NORMAL, corVermelho);

                    // GRAVA O FUNDO NO ARQUIVO
                    var pic = iTextSharp.text.Image.GetInstance(caminhoFundo);
                    pic.SetAbsolutePosition(0, 0);
                    pic.ScaleToFit(document.PageSize);
                    document.Add(pic);

                    // GRAVA O LOGO DA REDE
                    if (estabelecimento.Rede != null)
                    {
                        if (estabelecimento.Rede.Logo != null)
                        {
                            byte[] imageBytes = Convert.FromBase64String(estabelecimento.Rede.Logo);
                            File.WriteAllBytes(caminhoLogoRede, imageBytes);
                            if (File.Exists(caminhoLogoRede))
                            {
                                var logoRede = iTextSharp.text.Image.GetInstance(caminhoLogoRede);
                                logoRede.SetAbsolutePosition(196, 690);
                                logoRede.ScaleAbsoluteHeight(60);
                                logoRede.ScaleAbsoluteWidth(70);
                                logoRede.CompressionLevel = 8;
                                document.Add(logoRede);
                            }
                        }
                    }

                    // DADOS DO ESTABELECIMENTO
                    PdfContentByte directContent = worker.DirectContent;
                    ColumnText columnText = new ColumnText(directContent);
                    var posicaoDados = new Phrase(new Chunk(estabelecimento.EnderecoCompleto(), fontDadosEstabelecimento));
                    columnText.SetSimpleColumn(posicaoDados, 450, 580, 52, 680, fontDadosEstabelecimento.Size, Element.ALIGN_LEFT | Element.ALIGN_TOP);
                    columnText.Go();

                    // MES REFERENCIA
                    string textoMesReferencia = $"{estabelecimento.MesCompetencia} (META DE INCIDÊNCIA: ";
                    textoMesReferencia += $"{(int)(estabelecimento.ExtratoMesCompetencia.Meta * 100)}%)";
                    ColumnText mesReferencia = new ColumnText(directContent);
                    var mesReferenciaPhrase = new Phrase(new Chunk(textoMesReferencia, fontMesReferencia));
                    mesReferencia.SetSimpleColumn(mesReferenciaPhrase, 650, 590, 47, 620, fontMesReferencia.Size, 
                                                  Element.ALIGN_LEFT | Element.ALIGN_TOP);
                    mesReferencia.Go();

                    // TOTAL DE PEDIDOS
                    ColumnText totalPedido = new ColumnText(directContent);
                    string totalPedidos = estabelecimento.ExtratoMesCompetencia.TotalPedidos.ToString("N0");
                    var totalPedidoPhrase = new Phrase(new Chunk(totalPedidos, fontVendas));
                    totalPedido.SetSimpleColumn(totalPedidoPhrase, 100, 560, 47, 595, 25, Element.ALIGN_LEFT | Element.ALIGN_TOP);
                    totalPedido.Go();

                    // TOTAL DE PEDIDOS COM COCA
                    ColumnText totalPedidoCoca = new ColumnText(directContent);
                    string totalPedidosCoca = estabelecimento.ExtratoMesCompetencia.PedidosComCocaCola.ToString("N0");
                    var totalPedidoCocaPhrase = new Phrase(new Chunk(totalPedidosCoca, fontVendas));
                    totalPedidoCoca.SetSimpleColumn(totalPedidoCocaPhrase, 193, 560, 113, 595, 25, Element.ALIGN_LEFT | Element.ALIGN_TOP);
                    totalPedidoCoca.Go();

                    // INCIDENCIA
                    ColumnText incidenciaMes = new ColumnText(directContent);
                    string incidenciaText = ((int)(estabelecimento.ExtratoMesCompetencia.IncidenciaReal * 100)).ToString() + "%";
                    var incidenciaMesPhrase = new Phrase(new Chunk(incidenciaText, fontVendasBold));
                    incidenciaMes.SetSimpleColumn(incidenciaMesPhrase, 285, 560, 215, 595, 25, Element.ALIGN_LEFT | Element.ALIGN_TOP);
                    incidenciaMes.Go();

                    // RECEITA NAO CAPTURADA
                    ColumnText receitaMes = new ColumnText(directContent);
                    string receitaMesText = (estabelecimento.ExtratoMesCompetencia.ReceitaNaoCapturada * -1).ToString("C2");
                    var receitaMesPhrase = new Phrase(new Chunk(receitaMesText, fontVendasReceitaMes));
                    receitaMes.SetSimpleColumn(receitaMesPhrase, 500, 560, 300, 595, 25, Element.ALIGN_LEFT | Element.ALIGN_TOP);
                    receitaMes.Go();

                    // RECEITA TOTAL NAO CAPTURADA
                    ColumnText receitaTotalMes = new ColumnText(directContent);
                    string receitaTotalMesText = (estabelecimento.ExtratoVendas.Sum(x => x.ReceitaNaoCapturada) * -1).ToString("C2");
                    var receitaTotalMesPhrase = new Phrase(new Chunk(receitaTotalMesText, fontVendasReceitaTotal));
                    receitaTotalMes.SetSimpleColumn(receitaTotalMesPhrase, 500, 430, 300, 530, fontVendasReceitaMes.Size, Element.ALIGN_LEFT | Element.ALIGN_BOTTOM);
                    receitaTotalMes.Go();

                    // DADOS DA POSICAO
                    var posicaoTexto = $"{posicao.ToString()}º";
                    PdfContentByte cb = worker.DirectContent;
                    ColumnText ct = new ColumnText(cb);
                    var posicaoPhrase = new Phrase(new Chunk($"{posicao.ToString()}º", fontPosicaoRede));
                    ct.SetSimpleColumn(posicaoPhrase, 565, 515, 410, 565, fontPosicaoRede.Size, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    ct.Go();

                    const int FATOR_FIXO = 35;
                    int qtdExtrato = estabelecimento.ExtratoVendas.Count;
                    int fatorPosicao = (12 * FATOR_FIXO);

                    // MESES
                    ColumnText[] meses = new ColumnText[qtdExtrato];
                    string[] mesesText = new string[qtdExtrato];
                    Phrase[] mesesPhrase = new Phrase[qtdExtrato];

                    for (var index = qtdExtrato - 1; index >= 0; index--)
                    {
                        meses[index] = new ColumnText(directContent);
                        mesesText[index] = estabelecimento.ExtratoVendas.ElementAt(index).Competencia.ToString("MMM yy").PriMaiuscula();
                        mesesPhrase[index] = new Phrase(new Chunk(mesesText[index], fontMes));
                        fatorPosicao -= FATOR_FIXO;
                        meses[index].SetSimpleColumn(mesesPhrase[index], 240 + fatorPosicao, 375, 160 + fatorPosicao, 405, 25, Element.ALIGN_BOTTOM | Element.ALIGN_CENTER);
                        meses[index].Go();
                    }

                    // VOLUME PEDIDO
                    fatorPosicao = (12 * FATOR_FIXO);
                    ColumnText[] volumePedido = new ColumnText[qtdExtrato];
                    string[] volumePedidoText = new string[qtdExtrato];
                    Phrase[] volumePedidoPhrase = new Phrase[qtdExtrato];
                    for (var index = qtdExtrato - 1; index >= 0; index--)
                    {
                        volumePedido[index] = new ColumnText(directContent);
                        volumePedidoText[index] = estabelecimento.ExtratoVendas.ElementAt(index).TotalPedidos.ToString("N0");
                        volumePedidoPhrase[index] = new Phrase(new Chunk(volumePedidoText[index], fontValoresGraf));
                        fatorPosicao -= FATOR_FIXO;
                        volumePedido[index].SetSimpleColumn(volumePedidoPhrase[index], 240 + fatorPosicao, 360, 160 + fatorPosicao, 390, 25, Element.ALIGN_BOTTOM | Element.ALIGN_CENTER);
                        volumePedido[index].Go();
                    }

                    // VOLUME PEDIDO COM COCA
                    fatorPosicao = (12 * FATOR_FIXO);
                    ColumnText[] volumePedidoCoca = new ColumnText[qtdExtrato];
                    string[] volumePedidoCocaText = new string[qtdExtrato];
                    Phrase[] volumePedidoCocaPhrase = new Phrase[qtdExtrato];
                    for (var index = qtdExtrato - 1; index >= 0; index--)
                    {
                        volumePedidoCoca[index] = new ColumnText(directContent);
                        volumePedidoCocaText[index] = estabelecimento.ExtratoVendas.ElementAt(index).PedidosComCocaCola.ToString("N0");
                        volumePedidoCocaPhrase[index] = new Phrase(new Chunk(volumePedidoCocaText[index], fontValoresGraf));
                        fatorPosicao -= FATOR_FIXO;
                        volumePedidoCoca[index].SetSimpleColumn(volumePedidoCocaPhrase[index], 240 + fatorPosicao, 340, 160 + fatorPosicao, 370, 25, Element.ALIGN_BOTTOM | Element.ALIGN_CENTER);
                        volumePedidoCoca[index].Go();
                    }

                    // MESES2
                    ColumnText[] meses2 = new ColumnText[qtdExtrato];
                    string[] mesesText2 = new string[qtdExtrato];
                    Phrase[] mesesPhrase2 = new Phrase[qtdExtrato];
                    fatorPosicao = (12 * FATOR_FIXO);

                    for (var index = qtdExtrato - 1; index >= 0; index--)
                    {
                        meses2[index] = new ColumnText(directContent);
                        mesesText2[index] = estabelecimento.ExtratoVendas.ElementAt(index).Competencia.ToString("MMM yy").PriMaiuscula();
                        mesesPhrase2[index] = new Phrase(new Chunk(mesesText2[index], fontMes));
                        fatorPosicao -= FATOR_FIXO;
                        meses2[index].SetSimpleColumn(mesesPhrase[index], 240 + fatorPosicao, 160, 160 + fatorPosicao, 230, 25, Element.ALIGN_BOTTOM | Element.ALIGN_CENTER);
                        meses2[index].Go();
                    }

                    // PEDIDOS NAO CAPTURADOS
                    // Nota: o original usa vermelho-laranja (#FE0100) para negativos, verde (#0DA30D) para positivos
                    ColumnText[] naoCapiturados = new ColumnText[qtdExtrato];
                    string[] naoCapituradosText = new string[qtdExtrato];
                    Phrase[] naoCapituradosPhrase = new Phrase[qtdExtrato];

                    fatorPosicao = (12 * FATOR_FIXO);
                    for (var index = qtdExtrato - 1; index >= 0; index--)
                    {
                        int qtde = estabelecimento.ExtratoVendas.ElementAt(index).TotalPedidosNaoCapturados * -1;
                        Font fontNaoCap = qtde < 0 ? fontValoresGrafLaranja : qtde == 0 ? fontValoresGraf : fontValoresGrafGreen;
                        naoCapiturados[index] = new ColumnText(directContent);
                        naoCapituradosText[index] = qtde.ToString("N0");
                        naoCapituradosPhrase[index] = new Phrase(new Chunk(naoCapituradosText[index], fontNaoCap));
                        fatorPosicao -= FATOR_FIXO;
                        naoCapiturados[index].SetSimpleColumn(naoCapituradosPhrase[index], 240 + fatorPosicao, 140, 160 + fatorPosicao, 190, 25, Element.ALIGN_BOTTOM | Element.ALIGN_CENTER);
                        naoCapiturados[index].Go();
                    }

                    // PREÇO MÉDIO UNITÁRIO
                    ColumnText[] precoMedio = new ColumnText[qtdExtrato];
                    string[] precoMedioText = new string[qtdExtrato];
                    Phrase[] precoMedioPhrase = new Phrase[qtdExtrato];

                    fatorPosicao = (12 * FATOR_FIXO);
                    for (var index = qtdExtrato - 1; index >= 0; index--)
                    {
                        precoMedio[index] = new ColumnText(directContent);
                        precoMedioText[index] = estabelecimento.ExtratoVendas.ElementAt(index).PrecoUnitarioMedio.ToString("C2");
                        precoMedioPhrase[index] = new Phrase(new Chunk(precoMedioText[index], fontValoresGraf));
                        fatorPosicao -= FATOR_FIXO;
                        precoMedio[index].SetSimpleColumn(precoMedioPhrase[index], 240 + fatorPosicao, 115, 160 + fatorPosicao, 165, 25, Element.ALIGN_BOTTOM | Element.ALIGN_CENTER);
                        precoMedio[index].Go();
                    }

                    // RECEITA NAO CAPTURADOS
                    // Nota: o original usa vermelho #ED2224 para negativo, verde #0DA30D para positivo
                    ColumnText[] receitaNaoCapiturados = new ColumnText[qtdExtrato];
                    string[] receitaNaoCapituradosText = new string[qtdExtrato];
                    Phrase[] receitaNaoCapituradosPhrase = new Phrase[qtdExtrato];

                    fatorPosicao = (12 * FATOR_FIXO);
                    for (var index = qtdExtrato - 1; index >= 0; index--)
                    {
                        decimal receita = estabelecimento.ExtratoVendas.ElementAt(index).ReceitaNaoCapturada * -1;
                        Font fontNaoCap = receita < 0 ? fontValoresGrafRed : receita == 0 ? fontValoresGraf : fontValoresGrafGreen;
                        receitaNaoCapiturados[index] = new ColumnText(directContent);
                        receitaNaoCapituradosText[index] = receita.ToString("C2");
                        receitaNaoCapituradosPhrase[index] = new Phrase(new Chunk(receitaNaoCapituradosText[index], fontNaoCap));
                        fatorPosicao -= FATOR_FIXO;
                        receitaNaoCapiturados[index].SetSimpleColumn(receitaNaoCapituradosPhrase[index], 185 + fatorPosicao, 155, 155 + fatorPosicao, 210, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                        receitaNaoCapiturados[index].Go();
                    }

                    // PLOTAR A IMAGEM DO GRAFICO
                    var graficoImage = iTextSharp.text.Image.GetInstance(caminhoGrafico);
                    graficoImage.SetAbsolutePosition(152, 215);
                    graficoImage.ScaleAbsoluteHeight(120);
                    graficoImage.ScaleAbsoluteWidth(417);
                    document.Add(graficoImage);

                    // PLOTAR A IMAGEM DA SETA META
                    iTextSharp.text.Image setaMeta = iTextSharp.text.Image.GetInstance(caminhoSetaMeta);
                    setaMeta.SetAbsolutePosition(425, 420);
                    document.Add(setaMeta);

                    // Plotar Valor Meta
                    int metaValor = (int)(estabelecimento.ExtratoMesCompetencia.Meta * 100);
                    var columnTextMeta = new ColumnText(directContent);
                    var metaValorText = $"{metaValor.ToString("N0")}%";
                    var metaValorPhrase = new Phrase(new Chunk(metaValorText, fontCorMeta));
                    columnTextMeta.SetSimpleColumn(metaValorPhrase, 506, 485, 556, 455, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    columnTextMeta.Go();

                    int[,] incidenciaValorPosicao = { { 186, 460, 226, 430 }, { 196, 480, 246, 430 },
                                                      { 216, 500, 266, 470 }, { 262, 500, 308, 455 },
                                                      { 305, 495, 355, 465 }, { 325, 480, 365, 440 },
                                                      { 335, 460, 375, 430 }};
                    int indicePosicaoIncidenciaValor = 0;

                    // PLOTAR A IMAGEM DA SETA INCIDENCIA
                    iTextSharp.text.Image setaIncidencia;

                    if (estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.1) < 0)
                    {
                        setaIncidencia = iTextSharp.text.Image.GetInstance(caminhoSetaIncidencia5);
                    }
                    else if (estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.1) >= 0 &&
                             estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.35) < 0)
                    {
                        setaIncidencia = iTextSharp.text.Image.GetInstance(caminhoSetaIncidencia20);
                        indicePosicaoIncidenciaValor = 1;
                    }
                    else if (estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.35) >= 0 &&
                             estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.45) < 0)
                    {
                        setaIncidencia = iTextSharp.text.Image.GetInstance(caminhoSetaIncidencia40);
                        indicePosicaoIncidenciaValor = 2;
                    }
                    else if (estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.45) >= 0 &&
                             estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.55) < 0)
                    {
                        setaIncidencia = iTextSharp.text.Image.GetInstance(caminhoSetaIncidencia50);
                        indicePosicaoIncidenciaValor = 3;
                    }
                    else if (estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.55) >= 0 &&
                             estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.75) < 0)
                    {
                        setaIncidencia = iTextSharp.text.Image.GetInstance(caminhoSetaIncidencia60);
                        indicePosicaoIncidenciaValor = 4;
                    }
                    else if (estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.75) >= 0 &&
                             estabelecimento.ExtratoMesCompetencia.IncidenciaReal.CompareTo((decimal)0.95) < 0)
                    {
                        setaIncidencia = iTextSharp.text.Image.GetInstance(caminhoSetaIncidencia80);
                        indicePosicaoIncidenciaValor = 5;
                    }
                    else
                    {
                        setaIncidencia = iTextSharp.text.Image.GetInstance(caminhoSetaIncidencia95);
                        indicePosicaoIncidenciaValor = 6;
                    }

                    // Plotar Valor Incidência
                    int incValor = (int)(estabelecimento.ExtratoMesCompetencia.IncidenciaReal * 100);
                    var columnTextInc = new ColumnText(directContent);
                    var incValorText = $"{incValor.ToString("N0")}%";
                    var incValorPhrase = new Phrase(new Chunk(incValorText, fontCorIncidencia));
                    columnTextInc.SetSimpleColumn(incValorPhrase,
                                                  incidenciaValorPosicao[indicePosicaoIncidenciaValor, 0],
                                                  incidenciaValorPosicao[indicePosicaoIncidenciaValor, 1],
                                                  incidenciaValorPosicao[indicePosicaoIncidenciaValor, 2],
                                                  incidenciaValorPosicao[indicePosicaoIncidenciaValor, 3],
                                                  25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    columnTextInc.Go();

                    setaIncidencia.SetAbsolutePosition(230, 420);
                    document.Add(setaIncidencia);

                    // DESENHAR A LINHA TRACEJADA DA META
                    float posicaoMetaY = 225 + (float)(estabelecimento.ExtratoMesCompetencia.Meta * 97);
                    float posicaoIncidenciaY = 225 + (float)(estabelecimento.IncidenciaMedia * 97);
                    float posicaoTextoMeta = (float)(estabelecimento.ExtratoMesCompetencia.Meta >=
                                              estabelecimento.ExtratoMesCompetencia.IncidenciaReal ?
                                              posicaoMetaY + 5 : posicaoMetaY - 5);
                    float posicaoTextoIncidencia = (float)(estabelecimento.ExtratoMesCompetencia.IncidenciaReal >=
                                              estabelecimento.ExtratoMesCompetencia.Meta ?
                                              posicaoIncidenciaY + 5 : posicaoIncidenciaY - 5);

                    cb.SetLineDash(4.5f, 4.5f);
                    cb.SetRGBColorStroke(237, 34, 36);
                    cb.SetLineWidth(0.5f);
                    cb.MoveTo(105, posicaoMetaY);
                    cb.LineTo(565, posicaoMetaY);
                    cb.Stroke();
                    cb.SetColorStroke(BaseColor.BLACK);

                    // DESENHAR A LINHA TRACEJADA DA INCIDENCIA
                    cb.SetLineDash(4.5f, 4.5f);
                    cb.SetColorStroke(BaseColor.GRAY);
                    cb.MoveTo(105, posicaoIncidenciaY);
                    cb.LineTo(565, posicaoIncidenciaY);
                    cb.Stroke();
                    cb.SetColorStroke(BaseColor.BLACK);

                    // PALAVRA META NO GRAFICO
                    ColumnText textMeta = new ColumnText(directContent);
                    var textMetaPhrase = new Phrase(new Chunk("META", fontTextoMeta));
                    textMeta.SetSimpleColumn(textMetaPhrase, 110, posicaoTextoMeta, 30,
                                             posicaoTextoMeta, 0, Element.ALIGN_RIGHT);
                    textMeta.Go();

                    // PALAVRA INCIDENCIA NO GRAFICO
                    ColumnText textIncidencia = new ColumnText(directContent);
                    var textIncidenciaPhrase = new Phrase(new Chunk("INCIDÊNCIA REAL", fontTextoIncidencia));
                    textIncidencia.SetSimpleColumn(textIncidenciaPhrase, 110, posicaoTextoIncidencia - 5, 0,
                                                   posicaoTextoIncidencia - 5, 0, Element.ALIGN_RIGHT);
                    textIncidencia.Go();

                    // INCIDENCIA GRAFICO (% dentro das barras)
                    ColumnText[] incidenciaReal = new ColumnText[qtdExtrato];
                    string[] incidenciaRealText = new string[qtdExtrato];
                    Phrase[] incidenciaRealPhrase = new Phrase[qtdExtrato];

                    fatorPosicao = 12;
                    for (var index = qtdExtrato - 1; index >= 0; index--)
                    {
                        int incidenciaRealValor = (int)(estabelecimento.ExtratoVendas.ElementAt(index).IncidenciaReal * 100);
                        incidenciaReal[index] = new ColumnText(directContent);
                        incidenciaRealText[index] = $"{incidenciaRealValor.ToString("N0")}%";
                        incidenciaRealPhrase[index] = new Phrase(new Chunk(incidenciaRealText[index], fontValoresIncidencia));

                        switch (fatorPosicao)
                        {
                            case 1:
                                incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 159, 215, 179, 245, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                incidenciaReal[index].Go();
                                break;
                            case 2:
                                incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 192, 215, 212, 245, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                incidenciaReal[index].Go();
                                break;
                            case 3:
                                incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 228, 215, 248, 245, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                incidenciaReal[index].Go();
                                break;
                            case 4:
                                incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 263, 215, 283, 245, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                incidenciaReal[index].Go();
                                break;
                            case 5:
                                incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 298, 215, 318, 245, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                incidenciaReal[index].Go();
                                break;
                            case 6:
                                incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 333, 215, 353, 245, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                incidenciaReal[index].Go();
                                break;
                            case 7:
                                incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 368, 215, 388, 245, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                incidenciaReal[index].Go();
                                break;
                            case 8:
                                incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 405, 215, 425, 245, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                incidenciaReal[index].Go();
                                break;
                            case 9:
                                incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 438, 215, 458, 245, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                incidenciaReal[index].Go();
                                break;
                            case 10:
                                incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 474, 215, 494, 245, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                incidenciaReal[index].Go();
                                break;
                            case 11:
                                incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 509, 215, 529, 245, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                incidenciaReal[index].Go();
                                break;
                            case 12:
                                incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 544, 215, 564, 245, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                incidenciaReal[index].Go();
                                break;
                        }
                        fatorPosicao--;
                    }

                    filestream.Flush();
                    document.CloseDocument();

                    // PAGINA 2 - TEXTO EXPLICATIVO
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

        public void GerarGrafico(Estabelecimento estabelecimento, string contentRootPath)
        {
            var caminhoGrafico = Path.Combine(contentRootPath, "DadosApp", "Grafico.png");

            int largura = 500;
            int altura = 268;
            float margem = 2.0f;
            float larguraBarra = 33f;
            float espacamento = 9.0f;

            using var imagem = new Image<Rgba32>(largura, altura);
            imagem.Mutate(ctx =>
            {
                ctx.Fill(Color.White);

                // Barras
                int posicaoGrafico = 11;
                for (int i = estabelecimento.ExtratoVendas.Count() - 1; i >= 0; i--)
                {
                    // BARRA VERMELHA
                    float x = margem + posicaoGrafico * (larguraBarra + espacamento);
                    float y = altura - margem - (int)(250 * estabelecimento.ExtratoVendas.ElementAt(i).CorVermelhaGrafico);
                    var ret = new RectangleF(x, y, larguraBarra, (float)(250 * estabelecimento.ExtratoVendas.ElementAt(i).CorVermelhaGrafico));
                    ctx.Fill(Color.FromRgb(237, 34, 36), ret);

                    // BARRA VERDE
                    if (estabelecimento.ExtratoVendas.ElementAt(i).CorVerdeGrafico > 0)
                    {
                        float y2 = altura - margem - (float)(250 * estabelecimento.ExtratoVendas.ElementAt(i).CorVermelhaGrafico) -
                                   (float)(250 * estabelecimento.ExtratoVendas.ElementAt(i).CorVerdeGrafico * 1);
                        var ret2 = new RectangleF(x, y2, larguraBarra, (float)(250 * estabelecimento.ExtratoVendas.ElementAt(i).CorVerdeGrafico));
                        ctx.Fill(Color.FromRgb(13, 163, 13), ret2);
                    }
                    posicaoGrafico--;
                }
            });
            imagem.SaveAsPng(caminhoGrafico);
        }
    }
}
