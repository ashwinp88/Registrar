using DataAccessService.CommandHandlers;
using Domain.Commands;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using RegistrarAPI.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RegistrarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly Message message;


        public StudentController(Message message)
        {
            this.message = message;
        }
        // GET: api/<StudentController>
        [HttpGet]
        public async Task<IActionResult> GetList(string? studentName, string? courseName, int? courseCount)
        {
            var getStudentListQuery = new GetStudentListQuery(studentName, courseName, courseCount);
            return Ok(await message.Dispatch<List<StudentDTO>>(getStudentListQuery));
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getStudentByIdQuery = new GetStudentById(id);
            return Ok(await message.Dispatch<Student>(getStudentByIdQuery));
        }

        // POST api/<StudentController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student student)
        {
            var registerStudentCommand = new EnrollStudentCommand(student.Name, student.Email);
            await message.Dispatch(registerStudentCommand);
            return Ok();
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Student student)
        {
            var editStudentInfoCommand = new EditStudentInformationCommand(id, student.Name, student.Email);
            try
            {
                await message.Dispatch(editStudentInfoCommand);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var unEnrollStudentCommand = new UnEnrollStudentCommand(id);
            await message.Dispatch(unEnrollStudentCommand);
            return Ok();
        }

        // POST api/<StudentController>
        [HttpPost("RegisterCourse")]
        public async Task<IActionResult> RegisterCourse(int studentId, int courseId)
        {
            var registerCourseCommand = new RegisterCourseCommand(studentId, courseId);
            await message.Dispatch(registerCourseCommand);
            return Ok();
        }

        [HttpPost("UnregisterCourse")]
        public async Task<IActionResult> UnregisterCourse(int id, int courseId, string reason)
        {
            var unregisterCourseCommand = new UnRegisterCourseCommand(id, courseId, reason);    
            try
            {
                await message.Dispatch(unregisterCourseCommand);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
