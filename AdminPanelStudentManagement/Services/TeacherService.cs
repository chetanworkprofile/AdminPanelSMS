using AdminPanelStudentManagement.Data;
using AdminPanelStudentManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminPanelStudentManagement.Services
{
    public class TeacherService:ITeacherService
    {
        private readonly AdminSMSAPIDbContext DbContext;
        AuthService _authService;
        Response response = new Response();
        public TeacherService(AdminSMSAPIDbContext dbContext,IConfiguration configuration)
        {
            DbContext= dbContext;
            _authService = new AuthService(dbContext,configuration);
        }
        public async Task<Response> CreateTeacher(AddTeacher addTeacher)
        {
            var Teacher = new Teacher()
            {
                Id = Guid.NewGuid(),
                Name= addTeacher.Name,
                Email = addTeacher.Email,
                PasswordHash = _authService.CreatePasswordHash(addTeacher.Password),
                SubjectAllocated = addTeacher.SubjectAllocated,
                CreatedAt = DateTime.Now,
                UpdatedAt= DateTime.Now,
                IsDeleted= false
            };
            TeacherResponse teacherResponse = new TeacherResponse()
            {
                Id = Teacher.Id,
                Name = addTeacher.Name,
                Email = addTeacher.Email,
                SubjectAllocated = addTeacher.SubjectAllocated,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            await DbContext.Teachers.AddAsync(Teacher);
            await DbContext.SaveChangesAsync();
            response.StatusCode = 200;
            response.Message = "Teacher created";
            response.Data = teacherResponse;
            return response;
        }
    }
}
