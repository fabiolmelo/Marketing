namespace Marketing.Domain.Entidades
{
    public class MetaWebhookResponse
    {
        public MetaWebhookResponse()
        {
        }
        public MetaWebhookResponse(string response)
        {
            Response = response;
        }

        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
        public string? Response { get; private set; }
    }
}