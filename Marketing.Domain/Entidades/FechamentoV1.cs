namespace Marketing.Domain.Entidades
{
    public class FechamentoV1
    {
        public FechamentoV1(string nomeRede, byte[] fechamento)
        {
            NomeRede = nomeRede;
            Fechamento = fechamento;
        }

        public string NomeRede { get; private set; }
        public byte[] Fechamento { get; private set; }
    }
}