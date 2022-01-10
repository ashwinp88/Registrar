namespace Domain.Commands
{
    public interface ICommand 
    { 
    }

    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }

    public class EnrollStudentCommand : ICommand
    {
        public string StudentName { get; }
        public string Email { get; }
        public EnrollStudentCommand(string studentName, string email)
        {
            StudentName = studentName;
            Email = email;
        }
    }

    public class EditStudentInformationCommand : ICommand
    {
        public int Id { get; }
        public string StudentName { get; }
        public string Email { get; }
        public EditStudentInformationCommand(int id, string studentName, string email)
        {
            Id = id;
            StudentName = studentName;
            Email = email;
        }
    }

    public class RegisterCourseCommand : ICommand
    {
        public int StudentId { get; }
        public int CourseId { get; }
        public RegisterCourseCommand(int studentId, int courseId)
        {
            StudentId = studentId;
            CourseId = courseId;
        }
    }

    public class UnEnrollStudentCommand : ICommand
    {
        public UnEnrollStudentCommand(int studentId)
        {
            StudentId = studentId;
        }

        public int StudentId { get; }
    }
  
    public class UnRegisterCourseCommand : ICommand
    {
        public UnRegisterCourseCommand(int studentId, int courseId, string reason)
        {
            StudentId = studentId;
            CourseId = courseId;
            Reason = reason;
        }

        public int StudentId { get; }
        public int CourseId { get; }
        public string Reason { get; }
    }
}
