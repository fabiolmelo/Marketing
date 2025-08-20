namespace Marketing.Domain.Entidades
{
    public class WhatsAppMessageTemplate
    {
         public string messaging_product { get; set; }
        public string to { get; set; }
        public string type { get; set; }
        public Template template { get; set; }

        public WhatsAppMessageTemplate(string to, string templateName, string languageCode)
        {
            this.messaging_product = "whatsapp";
            this.to = to;
            this.type = "template";
            this.template = new Template(templateName, languageCode);
        }
    }

    public class Template
    {
        public Template(string name, string languageCode)
        {
            this.name = name;
            this.language = new Language(languageCode);
            this.components = new List<Component>();
        }

        public string name { get; set; }
        public Language language { get; private set; }
        public List<Component> components { get; set; }
    }

    public class Language
    {
        public Language(string code)
        {
            this.code = code;
        }

        public string code { get; set; }
    }
  
    public class Component
    {
        public string type { get; set; }
        public List<Parameter> parameters { get; set; }
        public string? sub_type { get; set; }
        public string? index { get; set; }

        public Component(string type)
        {
            this.type = type;
            this.parameters = new List<Parameter>();
        }
    }

    public class Parameter
    {
        public string type { get; set; }
        public string text { get; set; }

        public Parameter(string type, string text)
        {
            this.type = type;
            this.text = text;
        }
    }
}

