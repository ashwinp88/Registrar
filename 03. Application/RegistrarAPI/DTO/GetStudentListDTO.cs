namespace RegistrarAPI.DTO
{
    public class StudentDTO
    {
        public string? StudentName { get; set; }
        public string? Email { get; set; }
        public List<StudentCourseEnrollmentsDTO>? RegisteredCourses { get; set; }
    }

    public class StudentCourseEnrollmentsDTO
    {
        public string? CourseName { get; set; }
        public int Credits { get; set; }
    }
}
