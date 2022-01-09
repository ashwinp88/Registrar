namespace Domain.Entities;

public class Course : Entity 
{
    public string CourseName { get; private set;}
    public string? Description { get; set; }
    public int Credits { get; private set;}
    public Course(string courseName, int credits)
    {
        CourseName = courseName;
        Credits = credits;
    }
}