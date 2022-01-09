using DataAccessService;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RegistrarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        public RegistrarDbContext DbContext { get; }

        public CourseController(RegistrarDbContext dbContext)
        {
            DbContext = dbContext;
        }
        // GET: api/<CourseController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await DbContext.Courses.ToListAsync());
        }

        // GET api/<CourseController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await DbContext.Courses.FindAsync(id));
        }

        // POST api/<CourseController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Course course)
        {
            await DbContext.Courses.AddAsync(course);
            await DbContext.SaveChangesAsync();
            return Ok();
        }

        // PUT api/<CourseController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] string courseDescription)
        {
            var currentValue = await DbContext.Courses.FindAsync(id);
            if (currentValue is null)
                return NotFound();
            currentValue.Description = courseDescription;
            await DbContext.SaveChangesAsync();
            return Ok();
        }

        // DELETE api/<CourseController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var currentValue = await DbContext.Courses.FindAsync(id);
            if (currentValue is null)
                return NotFound();
            DbContext.Courses.Remove(currentValue);
            await DbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
