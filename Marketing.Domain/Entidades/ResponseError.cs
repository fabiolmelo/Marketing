namespace Marketing.Domain.DTOs
{
    public class ResponseError
    {
        public ResponseError(string planilha, int linha, string tipoErro, string erro)
        {
            Planilha = planilha;
            Linha = linha;
            TipoErro = tipoErro;
            Erro = erro;
        }

        public string Planilha { get; set; }
        public int Linha { get; set; }
        public string TipoErro { get; set; }
        public string Erro { get; set; }

        public override string ToString()
        {
            return $"Linha: {Linha} | Planilha: {Planilha} | Tipo do Erro: {TipoErro} | Detalhes: {Erro}";
        }
    }
}