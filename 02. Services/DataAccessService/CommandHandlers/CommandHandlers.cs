using Domain.Commands;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessService.CommandHandlers
{
    public class Message
    {
        private readonly IServiceProvider serviceProvider;

        public Message(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task Dispatch(ICommand command)
        {
            Type type = typeof(ICommandHandler<>);
            Type[] commandType = {command.GetType()};
            Type commandHandlerType = type.MakeGenericType(commandType);

            dynamic handler = serviceProvider.GetService(commandHandlerType)!;
            Task task = handler.HandleAsync((dynamic)command);
            await task;
        }

        public async Task<TResult> Dispatch<TResult>(IQuery query)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] commandType = { query.GetType(), typeof(TResult) };
            Type commandHandlerType = type.MakeGenericType(commandType);

            dynamic handler = serviceProvider.GetService(commandHandlerType)!;
            Task<TResult> task = handler.HandleAsync((dynamic)query);
            return await task;
        }
    }

    public class EnrollStudentCommandHandler : ICommandHandler<EnrollStudentCommand>
    {
        private readonly RegistrarDbContext dbContext;

        public EnrollStudentCommandHandler(RegistrarDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task HandleAsync(EnrollStudentCommand enrollStudentCommand)
        {
            var student = new Student(enrollStudentCommand.StudentName, enrollStudentCommand.Email);
            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();
        }
    }

    public class EditStudentInfotmationCommandHandler : ICommandHandler<EditStudentInformationCommand>
    {
        private readonly RegistrarDbContext dbContext;

        public EditStudentInfotmationCommandHandler(RegistrarDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task HandleAsync(EditStudentInformationCommand editStudentInfoCommand)
        {
            var currentValue = await dbContext.Students.FindAsync(editStudentInfoCommand.Id);
            if (currentValue is null)
                throw new InvalidCastException($"student with Id {editStudentInfoCommand.Id} not found");
            currentValue.Name = editStudentInfoCommand.StudentName;
            currentValue.Email = editStudentInfoCommand.Email;
            await dbContext.SaveChangesAsync();
        }
    }

    public class RegisterCourseCommandHandler : ICommandHandler<RegisterCourseCommand>
    {
        private readonly RegistrarDbContext dbContext;

        public RegisterCourseCommandHandler(RegistrarDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task HandleAsync(RegisterCourseCommand registerCourseCommand)
        {
            var student = await dbContext.Students
               .Include(s => s.CourseRegistrations)
               .ThenInclude(c => c.Course)
               .FirstAsync(s => s.Id == registerCourseCommand.StudentId);

            if (student is null)
                throw new InvalidOperationException($"Student with id={registerCourseCommand.StudentId} not found");

            var course = await dbContext.Courses.FindAsync(registerCourseCommand.CourseId);
            if (course is null)
                throw new InvalidOperationException($"Course with id={registerCourseCommand.CourseId} not found");

            student.RegisterCourse(course);
            await dbContext.SaveChangesAsync();
        }
    }

    public class UnEnrollStudentCommandHandler : ICommandHandler<UnEnrollStudentCommand>
    {
        private readonly RegistrarDbContext dbContext;

        public UnEnrollStudentCommandHandler(RegistrarDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task HandleAsync(UnEnrollStudentCommand command)
        {
            var currentValue = await dbContext.Students.FindAsync(command.StudentId);
            if (currentValue is null)
                throw new InvalidCastException($"student with Id {command.StudentId} not found");
            dbContext.Students.Remove(currentValue);
            await dbContext.SaveChangesAsync();
        }
    }

    public class UnRegisterCourseCommandHandler : ICommandHandler<UnRegisterCourseCommand>
    {
        private readonly RegistrarDbContext dbContext;

        public UnRegisterCourseCommandHandler(RegistrarDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task HandleAsync(UnRegisterCourseCommand command)
        {
            var student = await dbContext.Students
                 .Include(s => s.CourseRegistrations)
                 .Include(s => s.CourseUnregistrations)
                 .FirstAsync(s => s.Id == command.StudentId);

            if (student is null)
                throw new InvalidCastException($"student with Id {command.StudentId} not found");

            var course = await dbContext.Courses.FindAsync(command.CourseId);
            if (course is null)
                throw new InvalidCastException($"course with Id {command.CourseId} not found");

            student.UnregisterCourse(course, command.Reason);
            await dbContext.SaveChangesAsync();
        }
    }
}
