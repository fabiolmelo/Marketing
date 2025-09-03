namespace Marketing.Domain.Entidades
{
    public class WhatsAppResponseResult
    {
        public string? messaging_product { get; set; }
        public Contact[] contacts { get; set; } = Array.Empty<Contact>();
        public Message[] messages { get; set; } = Array.Empty<Message>();
    }
    public class Contact
    {
        public string? input { get; set; }
        public string? wa_id { get; set; }
    }

    public class Message
    {
        public string? id { get; set; }
    }
}