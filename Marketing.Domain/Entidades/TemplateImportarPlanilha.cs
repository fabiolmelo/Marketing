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

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not TemplateImportarPlanilha other)
                return false;
            return this.Id == other.Id && this.Template == other.Template;   
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Template) ;
        }
    }   
}