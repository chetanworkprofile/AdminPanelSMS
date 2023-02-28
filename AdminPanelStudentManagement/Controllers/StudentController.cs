using AdminPanelStudentManagement.Data;
using AdminPanelStudentManagement.Models;
using AdminPanelStudentManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystemAPI.Models;

namespace AdminPanelStudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        Response response = new Response();
        StudentService studentService;
        ILogger<TeacherController> _logger;
        public StudentController(AdminSMSAPIDbContext dbContext, IConfiguration configuration, ILogger<TeacherController> logger)
        {
            _logger = logger;
            studentService = new StudentService(dbContext, configuration);
        }


        [HttpPost]
        [Route("/api/v1/CreateStudent")]
        public ActionResult CreateStudent(AddStudent addStudent)
        {
            try
            {
                _logger.LogInformation("Create teacher method started");
                Task<Response> responseTask = studentService.CreateStudent(addStudent);
                response = responseTask.Result;
                if (response.StatusCode == 200)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error something wrong happened ", DateTime.Now);
                return StatusCode(500, $"Internal server error: {ex}"); ;
            }
        }

        [HttpGet]
        [Route("/api/v1/GetStudents")]
        public ActionResult GetStudents(Guid? StudentId = null, string? Name = null, string? Email = null, Guid? subjectId = null, String OrderBy = "Id", int SortOrder = 1, int RecordsPerPage = 10, int PageNumber = 0)
        {
            try
            {
                _logger.LogInformation("Get Students method started");
                Response response = studentService.GetStudents(StudentId, Name, Email, subjectId, OrderBy, SortOrder, RecordsPerPage, PageNumber);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error something wrong happened ", DateTime.Now);
                return StatusCode(500, $"Internal server error: {ex}"); ;
            }
        }

        [HttpPut]
        [Route("/api/v1/UpdateStudent")]
        public IActionResult UpdateStudent(Guid StudentId, UpdateStudent updateStudent)
        {
            try
            {
                _logger.LogInformation("Update Student method started");
                Response response = studentService.UpdateStudent(StudentId, updateStudent).Result;
                if (response.StatusCode == 200)
                {
                    return Ok(response);
                }
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error something wrong happened ", DateTime.Now);
                return StatusCode(500, $"Internal server error: {ex}"); ;
            }
        }

        [HttpDelete]
        [Route("/api/v1/DeleteStudent")]
        public IActionResult DeleteStudent(Guid StudentId)
        {
            try
            {
                _logger.LogInformation("Delete Student method started");
                Response response = studentService.DeleteStudent(StudentId).Result;
                if (response.StatusCode == 200)
                {
                    return Ok(response);
                }
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error something wrong happened ", DateTime.Now);
                return StatusCode(500, $"Internal server error: {ex}"); ;
            }
        }
    }
}
