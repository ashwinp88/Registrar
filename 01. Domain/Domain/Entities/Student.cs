using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities;

public class Student : Entity
{
    public string Name {get; set;}
    public string Email {get; set;}
    public ICollection<CourseRegistration> CourseRegistrations { get; private set; }
    public ICollection<CourseUnregistration> CourseUnregistrations { get; private set; }

    public Student(string name, string email)
    {
        Name = name ?? throw new System.ArgumentNullException(nameof(name));
        Email = email ?? throw new System.ArgumentNullException(nameof(email));
        CourseRegistrations = new List<CourseRegistration>();
        CourseUnregistrations = new List<CourseUnregistration>();
    }

    public void RegisterCourse(Course course) 
    {
        ArgumentNullException.ThrowIfNull(course);

        if(CourseRegistrations.Any(c => c.Course == course))
            throw new InvalidOperationException($"Course is already registered.");

        if (CourseRegistrations.Count ==2)
            throw new InvalidOperationException("A student cannot register more than 2 courses.");
        
        CourseRegistrations.Add(new CourseRegistration(this, course));
    }

    public void UnregisterCourse(Course course, string reason) 
    {
        ArgumentNullException.ThrowIfNull(course);
        ArgumentNullException.ThrowIfNull(reason);

        if (!CourseRegistrations.Any(c => c.Course == course))
            throw new InvalidOperationException("Student is not registered for this course.");
        
        if (CourseUnregistrations.Where(c => c.Course == course).Count() == 2)
            throw new InvalidOperationException("A student cannot un-register a course more than 2 times.");

        CourseRegistrations.Remove(CourseRegistrations.First(c => c.Course == course));
        CourseUnregistrations.Add(new CourseUnregistration(this, course, reason));
    }
}