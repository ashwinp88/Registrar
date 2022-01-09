using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace DataAccessService.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly RegistrarDbContext dBContext;
    public IRepository<Student> Students { get; private set; }
    public IRepository<Course> Courses { get; private set; }

    public UnitOfWork(string connectionString)
    {
        dBContext = new RegistrarDbContext(connectionString);
        //dBContext = new RegistrarDbContext();
        Students = new Repository<Student>(dBContext);
        Courses = new Repository<Course>(dBContext);
    }

    public void Dispose()
    {
        dBContext.Dispose();
    }

    public async Task Complete()
    {
        await dBContext.SaveChangesAsync();
    }

    public Task Rollback()
    {
        throw new NotImplementedException();
    }
}
