using HttpClientProject.Models;
using HttpClientProject.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HttpClientProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IApiService _apiService;

        public StudentsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: api/students
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _apiService.GetAsync<Student[]>("api/students");
            return Ok(students);
        }

        // GET: api/students/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _apiService.GetAsync<Student>($"api/students/{id}");
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        // POST: api/students
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdStudent = await _apiService.PostAsync<Student, Student>("api/students", student);
            return CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.Id }, createdStudent);
        }

        // PUT: api/students/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            if (id != student.Id)
            {
                return BadRequest("Student ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedStudent = await _apiService.PutAsync<Student, Student>($"api/students/{id}", student);
            return Ok(updatedStudent);
        }

        // DELETE: api/students/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _apiService.DeleteAsync($"api/students/{id}");
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
