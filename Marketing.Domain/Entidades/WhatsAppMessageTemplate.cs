namespace Marketing.Domain.Entidades
{
    public class WhatsAppMessageTemplate
    {
        public WhatsAppMessageTemplate(string to, string templateName, string languageCode)
        {
            this.to = to;
            this.template = new Template(templateName, languageCode);
        }

        public string messaging_product { get; private set; } = "whatsapp";
        public string to { get; private set; }
        public string type { get; private set; } = "template";
        public Template template { get; private set; }
    }

    public class Template
    {
        public Template(string name, string languageCode)
        {
            this.name = name;
            this.language = new Language(languageCode);
        }

        public string name { get; set; }
        public Language language { get; private set; }
    }

    public class Language
    {
        public Language(string code)
        {
            this.code = code;
        }

        public string code { get; set; }
    }
}

