namespace Marketing.Domain.Entidades
{
    public class GrupoRede
    {
        public GrupoRede(string cnpj, decimal incidenciaMedia)
        {
            Cnpj = cnpj;
            this.incidenciaMedia = incidenciaMedia;
        }

        public string Cnpj { get; set; }
        public decimal incidenciaMedia { get; set; }
    }
}