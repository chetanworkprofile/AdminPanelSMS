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
    public class SubjectController : ControllerBase
    {
        Response response = new Response();
        private readonly AdminSMSAPIDbContext DbContext;
        private readonly IConfiguration _configuration;
        SubjectService subjectService;
        ILogger<TeacherController> _logger;
        public SubjectController(AdminSMSAPIDbContext dbContext, IConfiguration configuration, ILogger<TeacherController> logger)
        {
            this.DbContext = dbContext;
            this._configuration = configuration;
            _logger = logger;
            subjectService = new SubjectService(dbContext, configuration);
        }


        [HttpPost]
        [Route("/api/v1/CreateSubject")]
        public ActionResult CreateSubject(AddSubject addsubject)
        {
            try
            {
                _logger.LogInformation("Create Subject method started");
                Task<Response> responseTask = subjectService.CreateSubject(addsubject);
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
        [Route("/api/v1/GetSubjects")]
        public ActionResult GetSubjects(int? subjectId = 0, string? Name = null, String OrderBy = "Id", int SortOrder = 1, int RecordsPerPage = 10, int PageNumber = 0)
        {
            try
            {
                _logger.LogInformation("Get Subjects method started");
                Response response = subjectService.GetSubjects(subjectId, Name, OrderBy, SortOrder, RecordsPerPage, PageNumber);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error something wrong happened ", DateTime.Now);
                return StatusCode(500, $"Internal server error: {ex}"); ;
            }
        }

        [HttpPut]
        [Route("/api/v1/UpdateSubject")]
        public IActionResult UpdateSubject(int SubjectId, [FromBody] UpdateSubject updateSubject)
        {
            try
            {
                _logger.LogInformation("Update subject method started");
                Response response = subjectService.UpdateSubject(SubjectId, updateSubject).Result;
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
        [Route("/api/v1/DeleteSubject")]
        public IActionResult DeleteSubject(int subjectId)
        {
            try
            {
                _logger.LogInformation("Delete Subject method started");
                Response response = subjectService.DeleteSubject(subjectId).Result;
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
