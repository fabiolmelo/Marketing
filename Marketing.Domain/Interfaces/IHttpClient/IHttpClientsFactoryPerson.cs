namespace Marketing.Domain.Interfaces.IHttpClient
{
    public interface IHttpClientsFactoryPerson
    {
        Dictionary<string, HttpClient> Clients();
        HttpClient Client(string key);
    }
}