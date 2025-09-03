namespace Marketing.Domain.Entidades.Meta
{
    public class WhatsAppResponseError
    {
        public Error error { get; set; } = new Error();
    }

    public class Error
    {
        public string? message { get; set; }
        public string? type { get; set; }
        public int? code { get; set; }
        public int? error_subcode { get; set; }
        public string? fbtrace_id { get; set; }
    }
}

