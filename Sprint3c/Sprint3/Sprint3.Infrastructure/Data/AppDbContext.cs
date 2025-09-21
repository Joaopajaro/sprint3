using Microsoft.EntityFrameworkCore;
using Sprint3.Core.Models;

namespace Sprint3.Infrastructure.Data
{
    /// <summary>
    /// Contexto de banco de dados usando EF Core.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Ativo> Ativos { get; set; } = null!;
        public DbSet<Carteira> Carteiras { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
