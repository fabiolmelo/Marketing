using System.Text;

namespace Marketing.Domain.DTOs
{
    public class ResponseValidation
    {
        public ResponseValidation()
        {
        }
        public List<ResponseError> Erros { get; set; } = new List<ResponseError>();
        public bool Valido { get { return Erros.Count == 0; } }

        public void AdicionarErro(string planilha, int linha, string tipoErro, string erro)
        {
            Erros.Add(new ResponseError(planilha, linha, tipoErro, erro));
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