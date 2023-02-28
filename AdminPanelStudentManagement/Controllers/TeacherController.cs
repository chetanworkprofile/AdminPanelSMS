using AdminPanelStudentManagement.Data;
using AdminPanelStudentManagement.Models;
using AdminPanelStudentManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System.Numerics;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using StudentManagementSystemAPI.Models;
using System.Data;

namespace AdminPanelStudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        Response response = new Response();
        TeacherService teacherService;
        ILogger<TeacherController> _logger;
        public TeacherController(AdminSMSAPIDbContext dbContext, IConfiguration configuration, ILogger<TeacherController> logger)
        {
            _logger = logger;
            teacherService = new TeacherService(dbContext, configuration);
        }


        [HttpPost]
        [Route("/api/v1/CreateTeacher")]
        public ActionResult CreateTeacher(AddTeacher addTeacher)
        {
            try
            {
                _logger.LogInformation("Create teacher method started");
                Task<Response> responseTask = teacherService.CreateTeacher(addTeacher);
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
        [Route("/api/v1/GetTeachers")]
        public ActionResult GetTeacher(Guid? TeacherID = null, string? Name = null, string? Email = null, Guid? subjectId = null , String OrderBy = "Id", int SortOrder = 1, int RecordsPerPage = 10, int PageNumber = 0)
        {
            try
            {
                _logger.LogInformation("Get Teachers method started");
                Response response = teacherService.GetTeachers(TeacherID, Name, Email, subjectId, OrderBy, SortOrder, RecordsPerPage, PageNumber);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error something wrong happened ", DateTime.Now);
                return StatusCode(500, $"Internal server error: {ex}"); ;
            }
        }

        [HttpPut]
        [Route("/api/v1/UpdateTeacher")]
        public IActionResult UpdateTeacher(Guid TeacherId, [FromBody] UpdateTeacher updateTeacher)
        {
            try
            {
                _logger.LogInformation("Update teacher method started");
                Response response = teacherService.UpdateTeacher(TeacherId, updateTeacher).Result;
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
        [Route("/api/v1/DeleteTeacher")]
        public IActionResult DeleteTeacher(Guid TeacherId)
        {
            try
            {
                _logger.LogInformation("Delete teacher method started");
                Response response = teacherService.DeleteTeacher(TeacherId).Result;
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

