namespace Marketing.Domain.Entidades
{
    public class GrupoRede
    {
        public GrupoRede(string cnpj, decimal incidenciaMedia)
        {
            Cnpj = cnpj;
            IncidenciaMedia = incidenciaMedia;
        }

        public string Cnpj { get; set; }
        public decimal IncidenciaMedia { get; set; }
    }
}