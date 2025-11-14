using System.Text;
using Marketing.Domain.Entidades;

namespace Marketing.Domain.DTOs
{
    public class ResponseValidation
    {
        public ResponseValidation()
        {
        }
        public List<ResponseError> Erros { get; private set; } = new List<ResponseError>();
        public List<DadosPlanilha> DadosPlanilhas { get; private set; } = new List<DadosPlanilha>();
        public bool Valido { get { return Erros.Count == 0; } }
        public bool FormatoInvalido  { get; set; }

        public void AdicionarErro(string planilha, int linha, string tipoErro, string erro)
        {
            Erros.Add(new ResponseError(planilha, linha, tipoErro, erro));
        }

        public void AdicionarErro(string planilha, int linha, string coluna, string tipoErro, string erro)
        {
            Erros.Add(new ResponseError(planilha, linha, coluna, tipoErro, erro));
        }

        public void AdicionarDados(DadosPlanilha dadosPlanilha)
        {
            DadosPlanilhas.Add(dadosPlanilha);
        }

        public void JoinAllDadosAndErros(List<DadosPlanilha> dados, List<ResponseError> erros)
        {
            DadosPlanilhas.AddRange(dados);
            Erros.AddRange(erros);
        }
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            if (Erros.Count > 0)
            {
                stringBuilder.AppendLine("Erros encontrados na importação");
                stringBuilder.AppendLine("");
            }
            foreach (var erro in Erros)
            {
                stringBuilder.AppendLine(erro.ToString());
            }
            return stringBuilder.ToString();
        }
    }
}