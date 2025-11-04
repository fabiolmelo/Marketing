namespace Marketing.Domain.Interfaces.Servicos
{
    public interface IServicoSeed
    {
        Task SeedConfiguracoesApp();
        Task SeedRedes();
    }
}