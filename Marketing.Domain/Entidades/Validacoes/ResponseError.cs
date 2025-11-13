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

        public ResponseError(string planilha, int linha, string coluna, string tipoErro, string erro)
        {
            Planilha = planilha;
            Linha = linha;
            Coluna = coluna;
            TipoErro = tipoErro;
            Erro = erro;
        }

        public string Planilha { get; set; }
        public int Linha { get; set; }
        public string? Coluna { get; set; }
        public string TipoErro { get; set; }
        public string Erro { get; set; }

        public override string ToString()
        {
            string coluna = $"Coluna: {Coluna ?? ""} | ";
            if (Coluna == null || Coluna.Length == 0) coluna = "";
            return $"Linha: {Linha} | Planilha: {Planilha} | {coluna}Tipo do Erro: {TipoErro} | Detalhes: {Erro}";
        }
    }
}