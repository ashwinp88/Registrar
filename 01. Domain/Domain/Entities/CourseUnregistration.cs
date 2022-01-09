namespace Domain.Entities;

public class CourseUnregistration : Entity
{
    public Student Student { get; private set; } = null!;
    public Course Course { get; private set; } = null!;
    public DateTime UnregistrationDate { get; private set; }
    public string Reason { get; private set; } = null!;

    private CourseUnregistration()
    {
    }

    public CourseUnregistration(Student student, Course course, string reason)
    {
        Student = student ?? throw new ArgumentNullException(nameof(student));
        Course = course ?? throw new ArgumentNullException(nameof(course));
        Reason = reason ?? throw new ArgumentNullException(nameof(reason));
        UnregistrationDate = DateTime.Now;
    }
}