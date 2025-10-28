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
    public class ServicoGraficoRevisado : IServicoGraficoRevisado
    {
        // private readonly IServicoEstabelecimento _servicoEstabelecimento;
        public ServicoGraficoRevisado(IServicoEstabelecimento servicoEstabelecimento)
        {
            //_servicoEstabelecimento = servicoEstabelecimento;
        }

        public string GerarArquivoPdf(Estabelecimento estabelecimento, string arquivoPdf, int posicao, string contentRootPath, string caminhoApp)
        {
            var caminhoFundo = Path.Combine(contentRootPath, "DadosApp", "FundoAtualizado.png");
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
            var caminhoLogoRede = Path.Combine(contentRootPath, "DadosApp", "Logos", "LogoTmp.png"); 


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
                    Font fontDadosEstabelecimento = FontFactory.GetFont("tccc-unityheadline-bold", BaseFont.CP1252, BaseFont.EMBEDDED, 6, Font.NORMAL, BaseColor.WHITE);
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


                    //GRAVA O LOGO DA REDE
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

                    //var sucesso = await _servicoEstabelecimento.AtualizarDadosCadastraisViaReceitaFederal(estabelecimento.Cnpj, false);
                    
                    // DADOS DO ESTABELECIMENTO
                    var dadosEstabelecimento1 = $"Loja: {estabelecimento.RazaoSocial}";
                    var dadosEstabelecimento2 = $"Cidade: {estabelecimento.Cidade} - {estabelecimento.Uf}";
                    string endereco = $"{estabelecimento.Endereco ?? ""},";
                    string endereco2 = $"{estabelecimento.Numero ?? ""} - {estabelecimento.Complemento ?? ""} - {estabelecimento.Bairro ?? ""}";
                    // if (sucesso)
                    // {
                    //     endereco = $"{estabelecimento.Endereco}, {estabelecimento.Numero}";
                    // }
                    var dadosEstabelecimento3 = $"Endereço: {endereco}";
                    var dadosEstabelecimento4 = $"Número..: {endereco2}";

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

                    //DADOS 4
                    // ColumnText columnText4 = new ColumnText(directContent);
                    // var posicaoDados4 = new Phrase(new Chunk(dadosEstabelecimento4, fontDadosEstabelecimento));
                    // columnText4.SetSimpleColumn(posicaoDados4, 650, 100, 50, 670, 25, Element.ALIGN_LEFT | Element.ALIGN_LEFT);
                    // columnText4.Go();

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
                    // ColumnText mesCompetencia = new ColumnText(directContent);
                    // string mesCompetenciaText = estabelecimento.Periodo();
                    // var mesCompetenciaPhrase = new Phrase(new Chunk(mesCompetenciaText, fontMes));
                    // mesCompetencia.SetSimpleColumn(mesCompetenciaPhrase, 350, 500, 45, 545, 25, Element.ALIGN_BOTTOM | Element.ALIGN_LEFT);
                    // mesCompetencia.Go();

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
                    ct.SetSimpleColumn(posicaoPhrase, 565, 555, 420, 590, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                    ct.Go();

                    const int FATOR_FIXO = 31;

                    // VOLUME PEDIDO
                    int qtdExtrato = estabelecimento.ExtratoVendas.Count;
                    ColumnText[] volumePedido = new ColumnText[qtdExtrato];
                    string[] volumePedidoText = new string[qtdExtrato];
                    Phrase[] volumePedidoPhrase = new Phrase[qtdExtrato];

                    int fatorPosicao = (12 * FATOR_FIXO);
                    for (var index = qtdExtrato-1; index >=0; index--)
                    {
                        volumePedido[index] = new ColumnText(directContent);
                        volumePedidoText[index] = estabelecimento.ExtratoVendas.ElementAt(index).TotalPedidos.ToString("N0");
                        volumePedidoPhrase[index] = new Phrase(new Chunk(volumePedidoText[index], fontValoresGraf));
                        fatorPosicao -= FATOR_FIXO;
                        volumePedido[index].SetSimpleColumn(volumePedidoPhrase[index], 285 + fatorPosicao, 382, 200 + fatorPosicao, 432, 25, Element.ALIGN_BOTTOM | Element.ALIGN_CENTER);
                        volumePedido[index].Go();
                    }

                    // VOLUME PEDIDO COM COCA
                    ColumnText[] volumePedidoCoca = new ColumnText[qtdExtrato];
                    string[] volumePedidoCocaText = new string[qtdExtrato];
                    Phrase[] volumePedidoCocaPhrase = new Phrase[qtdExtrato];

                    fatorPosicao = (12 * FATOR_FIXO);
                    for (var index = qtdExtrato-1; index >=0; index--)
                    {
                        volumePedidoCoca[index] = new ColumnText(directContent);
                        volumePedidoCocaText[index] = estabelecimento.ExtratoVendas.ElementAt(index).PedidosComCocaCola.ToString("N0");
                        volumePedidoCocaPhrase[index] = new Phrase(new Chunk(volumePedidoCocaText[index], fontValoresGraf));
                        fatorPosicao -= FATOR_FIXO;
                        volumePedidoCoca[index].SetSimpleColumn(volumePedidoCocaPhrase[index], 285 + fatorPosicao, 367, 200 + fatorPosicao, 417, 25, Element.ALIGN_BOTTOM | Element.ALIGN_CENTER);
                        volumePedidoCoca[index].Go();
                    }

                    // MESES 2
                    ColumnText[] meses2 = new ColumnText[qtdExtrato];
                    string[] mesesText2 = new string[qtdExtrato];
                    Phrase[] mesesPhrase2 = new Phrase[qtdExtrato];

                    fatorPosicao = (12 * FATOR_FIXO);
                    for (var index = qtdExtrato-1; index >=0; index--)
                    {
                        meses2[index] = new ColumnText(directContent);
                        mesesText2[index] = estabelecimento.ExtratoVendas.ElementAt(index).Competencia.ToString("MMM yy").PriMaiuscula();
                        mesesPhrase2[index] = new Phrase(new Chunk(mesesText2[index], fontValoresGraf));
                        fatorPosicao -= FATOR_FIXO;
                        meses2[index].SetSimpleColumn(mesesPhrase2[index], 285 + fatorPosicao, 397, 200 + fatorPosicao, 447, 25, Element.ALIGN_BOTTOM | Element.ALIGN_CENTER);
                        meses2[index].Go();
                    }

                    // MESES
                    ColumnText[] meses = new ColumnText[qtdExtrato];
                    string[] mesesText = new string[qtdExtrato];
                    Phrase[] mesesPhrase = new Phrase[qtdExtrato];

                    fatorPosicao = (12 * FATOR_FIXO);
                    for (var index = qtdExtrato-1; index >=0; index--)
                    {
                        meses[index] = new ColumnText(directContent);
                        mesesText[index] = estabelecimento.ExtratoVendas.ElementAt(index).Competencia.ToString("MMM yy").PriMaiuscula();
                        mesesPhrase[index] = new Phrase(new Chunk(mesesText[index], fontValoresGraf));
                        fatorPosicao -= FATOR_FIXO;
                        meses[index].SetSimpleColumn(mesesPhrase[index], 285 + fatorPosicao, 212, 200 + fatorPosicao, 272, 25, Element.ALIGN_BOTTOM | Element.ALIGN_CENTER);
                        meses[index].Go();
                    }

                    // APROVEITAMENTO
                    ColumnText[] aproveitamento = new ColumnText[qtdExtrato];
                    string[] aproveitamentoText = new string[qtdExtrato];
                    Phrase[] aproveitamentoPhrase = new Phrase[qtdExtrato];

                    fatorPosicao = (12 * FATOR_FIXO);
                    for (var index = qtdExtrato-1; index >=0; index--)
                    {
                        decimal aprovMeta = estabelecimento.ExtratoVendas.ElementAt(index).IncidenciaReal - estabelecimento.ExtratoVendas.ElementAt(index).Meta;
                        aproveitamento[index] = new ColumnText(directContent);
                        aproveitamentoText[index] = aprovMeta.ToString("P0");
                        aproveitamentoPhrase[index] = new Phrase(new Chunk(aproveitamentoText[index], fontValoresGraf));
                        fatorPosicao -= FATOR_FIXO;
                        aproveitamento[index].SetSimpleColumn(aproveitamentoPhrase[index], 197 + fatorPosicao, 195, 217 + fatorPosicao, 252, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                        aproveitamento[index].Go();
                    }
                    
                    // PEDIDOS NAO CAPTURADOS
                    ColumnText[] naoCapiturados = new ColumnText[qtdExtrato];
                    string[] naoCapituradosText = new string[qtdExtrato];
                    Phrase[] naoCapituradosPhrase = new Phrase[qtdExtrato];

                    fatorPosicao = (12 * FATOR_FIXO);
                    for (var index = qtdExtrato-1; index >=0; index--)
                    {
                        int qtde = estabelecimento.ExtratoVendas.ElementAt(index).TotalPedidosNaoCapturados * -1;
                        Font fontNaoCap = qtde < 0 ? fontValoresGrafRed : qtde == 0 ? fontValoresGraf : fontValoresGrafGreen;
                        naoCapiturados[index] = new ColumnText(directContent);
                        naoCapituradosText[index] = qtde.ToString("N0");
                        naoCapituradosPhrase[index] = new Phrase(new Chunk(naoCapituradosText[index], fontNaoCap));
                        fatorPosicao -= FATOR_FIXO;
                        naoCapiturados[index].SetSimpleColumn(naoCapituradosPhrase[index], 197 + fatorPosicao, 173, 217 + fatorPosicao, 228, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                        naoCapiturados[index].Go();
                    }

                    // PREÇO MÉDIO UNITÁRIO
                    ColumnText[] precoMedio = new ColumnText[qtdExtrato];
                    string[] precoMedioText = new string[qtdExtrato];
                    Phrase[] precoMedioPhrase = new Phrase[qtdExtrato];

                    fatorPosicao = (12 * FATOR_FIXO);
                    for (var index = qtdExtrato-1; index >=0; index--)
                    {
                        precoMedio[index] = new ColumnText(directContent);
                        precoMedioText[index] = estabelecimento.ExtratoVendas.ElementAt(index).PrecoUnitarioMedio.ToString("C2");
                        precoMedioPhrase[index] = new Phrase(new Chunk(precoMedioText[index], fontValoresGraf));
                        fatorPosicao -= FATOR_FIXO;
                        precoMedio[index].SetSimpleColumn(precoMedioPhrase[index], 197 + fatorPosicao, 173, 217 + fatorPosicao, 211, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                        precoMedio[index].Go();
                    }

                    // CIFRÃO FIXO
                    ColumnText[] crifaofixo = new ColumnText[qtdExtrato];
                    string[] crifaofixoText = new string[qtdExtrato];
                    Phrase[] crifaofixoPhrase = new Phrase[qtdExtrato];

                    fatorPosicao = (12 * FATOR_FIXO);
                    for (var index = qtdExtrato-1; index >=0; index--)
                    {
                        decimal receita = estabelecimento.ExtratoVendas.ElementAt(index).ReceitaNaoCapturada * -1;
                        Font fontNaoCap = receita < 0 ? fontValoresGrafRed : receita == 0 ? fontValoresGraf : fontValoresGrafGreen;
                        crifaofixo[index] = new ColumnText(directContent);
                        crifaofixoText[index] = "R$";
                        crifaofixoPhrase[index] = new Phrase(new Chunk(crifaofixoText[index], fontNaoCap));
                        fatorPosicao -= FATOR_FIXO;
                        crifaofixo[index].SetSimpleColumn(crifaofixoPhrase[index], 203 + fatorPosicao, 167, 213 + fatorPosicao, 197, 25, Element.ALIGN_TOP | Element.ALIGN_CENTER);
                        crifaofixo[index].Go();
                    }

                    // RECEITA NAO CAPTURADOS
                    ColumnText[] receitaNaoCapiturados = new ColumnText[qtdExtrato];
                    string[] receitaNaoCapituradosText = new string[qtdExtrato];
                    Phrase[] receitaNaoCapituradosPhrase = new Phrase[qtdExtrato];

                    fatorPosicao = (12 * FATOR_FIXO);
                    for (var index = qtdExtrato-1; index >=0; index--)
                    {
                        decimal receita = estabelecimento.ExtratoVendas.ElementAt(index).ReceitaNaoCapturada * -1;
                        Font fontNaoCap = receita < 0 ? fontValoresGrafRed : receita == 0 ? fontValoresGraf : fontValoresGrafGreen;
                        receitaNaoCapiturados[index] = new ColumnText(directContent);
                        receitaNaoCapituradosText[index] = receita.ToString("N2");
                        receitaNaoCapituradosPhrase[index] = new Phrase(new Chunk(receitaNaoCapituradosText[index], fontNaoCap));
                        fatorPosicao -= FATOR_FIXO;
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
                    setaMeta.SetAbsolutePosition(425, 445);
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
                    
                    setaIncidencia.SetAbsolutePosition(275, 445);
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

                    fatorPosicao = 12;
                    for (var index = qtdExtrato - 1; index >= 0; index--)
                    {
                        int incidenciaRealValor = (int)(estabelecimento.ExtratoVendas.ElementAt(index).IncidenciaReal * 100);
                        incidenciaReal[index] = new ColumnText(directContent);
                        incidenciaRealText[index] = $"{incidenciaRealValor.ToString("N0")}%";
                        incidenciaRealPhrase[index] = new Phrase(new Chunk(incidenciaRealText[index], fontValoresIncidencia));

                        while (fatorPosicao > 0)
                        {
                            switch (fatorPosicao)
                            {
                                case 1:
                                    incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 200, 265, 218, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                    incidenciaReal[index].Go();
                                    break;
                                case 2:
                                    incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 230, 265, 250, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                    incidenciaReal[index].Go();
                                    break;
                                case 3:
                                    incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 261, 265, 281, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                    incidenciaReal[index].Go();
                                    break;
                                case 4:
                                    incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 290, 265, 310, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                    incidenciaReal[index].Go();
                                    break;
                                case 5:
                                    incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 322, 265, 342, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                    incidenciaReal[index].Go();
                                    break;
                                case 6:
                                    incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 354, 265, 372, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                    incidenciaReal[index].Go();
                                    break;
                                case 7:
                                    incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 382, 265, 402, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                    incidenciaReal[index].Go(); 
                                    break;
                                case 8:
                                    incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 414, 265, 432, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                    incidenciaReal[index].Go();
                                    break;
                                case 9:
                                    incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 446, 265, 464, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                    incidenciaReal[index].Go();
                                    break;
                                case 10:
                                    incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 476, 265, 494, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                    incidenciaReal[index].Go();
                                    break;
                                case 11:
                                    incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 506, 265, 524, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                    incidenciaReal[index].Go();
                                    break;
                                case 12:
                                    incidenciaReal[index].SetSimpleColumn(incidenciaRealPhrase[index], 536, 265, 555, 295, 25, Element.ALIGN_CENTER | Element.ALIGN_CENTER);
                                    incidenciaReal[index].Go();
                                    break;
                            }
                            fatorPosicao--;
                        }
                    }

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

        public void GerarGrafico(Estabelecimento estabelecimento, string contentRootPath)
        {
            var caminhoGrafico = Path.Combine(contentRootPath, "DadosApp", "Grafico.jpg");

            int largura = 500;
            int altura = 268;
            float margem = 7.5f;
            float larguraBarra = 31f;
            float espacamento = 9.5f;

            using var imagem = new Image<Rgba32>(largura, altura);
            imagem.Mutate(ctx =>
            {
                ctx.Fill(Color.White);

                // Barras
                int posicaoGrafico = 11;
                for (int i = estabelecimento.ExtratoVendas.Count()-1; i >=0; i--)
                {
                    // BARRA VERMELHA
                    float x = margem + posicaoGrafico * (larguraBarra + espacamento);
                    float y = altura - margem - (int)(230 * estabelecimento.ExtratoVendas.ElementAt(i).CorVermelhaGrafico);
                    var ret = new RectangleF(x, y, larguraBarra, (float)(230 * estabelecimento.ExtratoVendas.ElementAt(i).CorVermelhaGrafico));
                    ctx.Fill(Color.FromRgb(237, 34, 36), ret);

                    // BARRA VERDE
                    if (estabelecimento.ExtratoVendas.ElementAt(i).CorVerdeGrafico > 0)
                    {
                        float y2 = altura - margem - (float)(230 * estabelecimento.ExtratoVendas.ElementAt(i).CorVermelhaGrafico) -
                                   (float)(230 * estabelecimento.ExtratoVendas.ElementAt(i).CorVerdeGrafico * 1);
                        var ret2 = new RectangleF(x, y2, larguraBarra, (float)(230 * estabelecimento.ExtratoVendas.ElementAt(i).CorVerdeGrafico));
                        ctx.Fill(Color.FromRgb(13, 163, 13), ret2);
                    }
                    posicaoGrafico--;
                }
            });
            imagem.SaveAsJpeg(caminhoGrafico);
        }
    }
}