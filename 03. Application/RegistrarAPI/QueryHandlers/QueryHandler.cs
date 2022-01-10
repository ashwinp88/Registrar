using DataAccessService;
using Domain.Commands;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using RegistrarAPI.DTO;

namespace RegistrarAPI.QueryHandlers
{
    public class GetStudentsListHandler : IQueryHandler<GetStudentListQuery, List<StudentDTO>>
    {
        private readonly RegistrarDbContext dbContext;

        public GetStudentsListHandler(RegistrarDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<StudentDTO>?> HandleAsync(GetStudentListQuery query)
        {
            IQueryable<Student> students = dbContext.Students
                    .Include(s => s.CourseRegistrations)
                    .ThenInclude(c => c.Course);

            if (query.StudentName != null)
                students = students.Where(s => s.Name == query.StudentName);

            if (query.CourseName != null)
                students = students
                    .Where(s => s.CourseRegistrations
                    .Any(c => c.Course.CourseName == query.CourseName));

            if (query.CourseCount > 0)
                students = students.Where(s => s.CourseRegistrations.Count == query.CourseCount);

            // Construct Result DTO
            var results = await students.ToListAsync();
            List<StudentDTO> studentList = new List<StudentDTO>();
            foreach (var result in results)
            {
                var studentDTO = new StudentDTO() { StudentName = result.Name, Email = result.Email };
                studentDTO.RegisteredCourses = new List<StudentCourseEnrollmentsDTO>();
                foreach (var courseRegistration in result.CourseRegistrations)
                {
                    var courseEnrollment = new StudentCourseEnrollmentsDTO()
                    {
                        CourseName = courseRegistration.Course.CourseName,
                        Credits = courseRegistration.Course.Credits
                    };
                    studentDTO.RegisteredCourses.Add(courseEnrollment);
                }
                studentList.Add(studentDTO);
            }
            return studentList;
        }

    }

    public class GetStudentByIdHandler : IQueryHandler<GetStudentById, Student>
    {
        private readonly RegistrarDbContext dbContext;

        public GetStudentByIdHandler(RegistrarDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Student?> HandleAsync(GetStudentById query)
        {
            return await dbContext.Students.FindAsync(query.StudentId);
        }
    }
}
