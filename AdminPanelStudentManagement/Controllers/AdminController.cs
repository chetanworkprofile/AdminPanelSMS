using AdminPanelStudentManagement.Data;
using AdminPanelStudentManagement.Models;
using AdminPanelStudentManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanelStudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        AuthService authService;
        TeacherService teacherService;
        Response response = new Response();
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger,AdminSMSAPIDbContext dbContext,IConfiguration configuration)
        {
            authService = new AuthService(dbContext,configuration);
            teacherService = new TeacherService(dbContext,configuration);
            _logger = logger;
        }
        [HttpPost]
        [Route("/api/v1/adminlogin")]
        public ActionResult AdminLogin(UserDTO request)
        {
            _logger.LogInformation("Admin Login attempt");
            response = authService.AdminLogin(request);
            if (response.StatusCode == 404)
            {
                _logger.LogError(response.Message);
                return BadRequest(response);
            }
            else if (response.StatusCode == 403)
            {
                _logger.LogError(response.Message);
                return BadRequest(response);
            }
            return Ok(response);
        }
/*
        [HttpPost]
        [Route("/api/v1/CreateTeacher")]*/
        /*public ActionResult CreateTeacher(AddTeacher addTeacher)
        {

        }*/
    }
}
