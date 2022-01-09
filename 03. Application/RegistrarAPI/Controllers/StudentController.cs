using DataAccessService;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RegistrarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        public RegistrarDbContext DbContext { get; }

        public StudentController(RegistrarDbContext dbContext)
        {
            DbContext = dbContext;
        }
        // GET: api/<StudentController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await DbContext.Students.ToListAsync());
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await DbContext.Students.FindAsync(id));
        }

        // POST api/<StudentController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student student)
        {
            await DbContext.Students.AddAsync(student);
            await DbContext.SaveChangesAsync();
            return Ok();
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Student student)
        {
            var currentValue = await DbContext.Students.FindAsync(id);
            if (currentValue is null)
                return NotFound();
            currentValue.Name = student.Name;
            currentValue.Email = student.Email;
            await DbContext.SaveChangesAsync();
            return Ok();
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var currentValue = await DbContext.Students.FindAsync(id);
            if (currentValue is null)
                return NotFound();
            DbContext.Students.Remove(currentValue);
            await DbContext.SaveChangesAsync();
            return Ok();
        }

        // POST api/<StudentController>
        [HttpPost("RegisterCourse")]
        public async Task<IActionResult> RegisterCourse(int id, int courseId)
        {
            var student = await DbContext.Students
               .Include(s => s.CourseRegistrations)
               .FirstAsync(s => s.Id == id);

            if (student is null)
                return NotFound($"Student with id={id} not found");

            var course = await DbContext.Courses.FindAsync(courseId);
            if (course is null)
                return NotFound($"Course with id={courseId} not found");

            try
            {
                student.RegisterCourse(course);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPost("UnregisterCourse")]
        public async Task<IActionResult> UnregisterCourse(int id, int courseId, string reason)
        {
            var student = await DbContext.Students
                .Include(s => s.CourseRegistrations)
                .Include(s => s.CourseUnregistrations)
                .FirstAsync(s => s.Id == id);
            
            if (student is null)
                return NotFound($"Student with id={id} not found");

            var course = await DbContext.Courses.FindAsync(courseId);
            if (course is null)
                return NotFound($"Course with id={courseId} not found");

            try
            {
                student.UnregisterCourse(course, reason);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
