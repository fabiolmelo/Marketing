namespace Marketing.Domain.Entidades
{
    public class WhatsAppMessageTemplate
    {
        public WhatsAppMessageTemplate(string to, string templateName, string languageCode)
        {
            this.to = to;
            this.template = new Template(templateName, languageCode);
            this.type = "template";
        }

        public string messaging_product { get; private set; } = "whatsapp";
        public string to { get; private set; }
        public string type { get; private set; }
        public Template template { get; private set; }
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
        public Component(string type)
        {
            this.type = type;
            this.parameters = new List<Parameter>();
        }

        public string type { get; set; }
        public List<Parameter> parameters { get; set; }
    }

    public class Parameter
    {
        public Parameter(string type, string text)
        {
            this.type = type;
            this.text = text;
        }

        public string type { get; set; }
        public string text { get; set; }
    }
}

