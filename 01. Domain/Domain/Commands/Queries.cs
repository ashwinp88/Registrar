using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commands
{
    public interface IQuery { }

    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery
    {
        Task<TResult?> HandleAsync(TQuery query);
    }

    public class GetStudentListQuery : IQuery
    {
        public string? StudentName { get; }
        public string? CourseName { get; }
        public int? CourseCount { get; }
        public GetStudentListQuery(string? studentName, string? courseName, int? courseCount)
        {
            StudentName = studentName;
            CourseName = courseName;
            CourseCount = courseCount;
        }
    }

    public class GetStudentById : IQuery
    {
        public GetStudentById(int studentId)
        {
            StudentId = studentId;
        }

        public int StudentId { get; }
    }
}
