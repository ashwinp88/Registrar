using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessService;
public class RegistrarDbContext : DbContext
{
    public virtual DbSet<Student> Students { get; set; } = null!;
    public virtual DbSet<Course> Courses { get; set; } = null!;

    public RegistrarDbContext(string connectionString) 
        : this(new DbContextOptionsBuilder<RegistrarDbContext>().UseSqlServer(connectionString).Options)
    {
    }

    public RegistrarDbContext(DbContextOptions<RegistrarDbContext> options) : base(options)
    {
    }
}
