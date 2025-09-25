using Marketing.Domain.Entidades;
using Marketing.Domain.Interfaces.Servicos;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace Marketing.Application.Servicos
{
    public class ServicoGrafico : IServicoGrafico
    {
        public ServicoGrafico()
        {
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
                for (int i = 0; i < estabelecimento.ExtratoVendas.Count(); i++)
                {
                    // BARRA VERMELHA
                    float x = margem + i * (larguraBarra + espacamento);
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
                }
            });
            imagem.SaveAsJpeg(caminhoGrafico);
        }
    }
}



        