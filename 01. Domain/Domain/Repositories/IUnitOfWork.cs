using System;
using Domain.Entities;

namespace Domain.Repositories;
 public interface IUnitOfWork : IDisposable
 {
    public IRepository<Student> Students { get; }
    public IRepository<Course> Courses { get; }
    //public IRepository<CourseRegistration> CourseRegistrations { get; }
    public Task Complete();
    public Task Rollback();
 }