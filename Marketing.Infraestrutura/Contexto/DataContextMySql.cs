using Microsoft.EntityFrameworkCore;

namespace Marketing.Infraestrutura.Contexto
{
    public class DataContextMySql : DbContext
    {
        public DataContextMySql(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
    }
}