namespace Domain.Entities;

public class CourseRegistration : Entity
{
    public Student Student { get; private set; } = null!;
    public Course Course { get; private set; } = null!;
    public DateTime RegistrationDate { get; private set; }

    private CourseRegistration()
    {

    }

    public CourseRegistration(Student student, Course course) 
    {
        Student = student ?? throw new ArgumentNullException(nameof(student));
        Course = course ?? throw new ArgumentNullException(nameof(course));
        RegistrationDate = DateTime.Now;
    }
}