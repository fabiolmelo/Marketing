namespace Marketing.Domain.Entidades
{
    public enum MensagemStatus
    {
        INQUEUE = 0,        
        SENT = 1,
        DELIVERED = 2,
        READ = 3,
        CLICKLINK = 4,
        FAILED = 5,
    }
}