namespace Marketing.Domain.Entidades
{
    public class TemplateImportarPlanilha
    {
        public TemplateImportarPlanilha(int id, string template)
        {
            Id = id;
            Template = template;
        }

        public int Id { get; set; }
        public string Template { get; set; }
    }
}