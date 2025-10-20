namespace Marketing.Domain.Entidades
{
    public class ConfiguracaoApp
    {
        public ConfiguracaoApp()
        {
        }
        
        public int Id { get; set; }
        public string? AppUrl { get; set; }
        public string? MetaApiUrl { get; set; }
        public string? MetaToken { get; set; }
        public string? FoneFrom { get; set; }
        public int? LoteProcessamento { get; set; }
        public int? IntervaloEntreDisparos { get; set; }
    }
}