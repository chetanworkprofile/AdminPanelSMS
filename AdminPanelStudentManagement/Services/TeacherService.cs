using AdminPanelStudentManagement.Data;
using AdminPanelStudentManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using StudentManagementSystemAPI.Models;
using System.Collections.Generic;
using System.Text.Json;

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
            //List<Subject>? subjects = await DbContext.SubjectTeachersMappings.Where(st=> st.tea);
            Teacher? tempTeacher = await DbContext.Teachers.FindAsync(addTeacher.Email);
            if(tempTeacher == null)
            {
                var Teacher = new Teacher()
                {
                    Id = Guid.NewGuid(),
                    Name = addTeacher.Name,
                    Email = addTeacher.Email,
                    PasswordHash = _authService.CreatePasswordHash(addTeacher.Password),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false
                };
                TeacherResponse teacherResponse = new TeacherResponse()
                {
                    Id = Teacher.Id,
                    Name = addTeacher.Name,
                    Email = addTeacher.Email,
                    SubjectsAllocated = new List<Subject>(),
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
            response.StatusCode = 409;
            response.Message = "Email already registered";
            response.Data = tempTeacher;
            return response;
        }

        public Response GetTeachers(Guid? TeacherID, string? Name, string? Email, Guid? subjectId, String OrderBy, int SortOrder, int RecordsPerPage, int PageNumber)          // sort order   ===   e1 for ascending   -1 for descending
        {
            var teacher = DbContext.Teachers;
            teacher  = (DbSet<Teacher>)teacher.Where(t => t.IsDeleted == false);
            if (TeacherID != null) { teacher = (DbSet<Teacher>)teacher.Where(s => (s.Id == TeacherID)); }
            if (Name != null) { teacher = (DbSet<Teacher>)teacher.Where(s => (s.Name == Name)); }
            if (Email != null) { teacher = (DbSet<Teacher>)teacher.Where(s => (s.Email == Email)); }
            List<Guid> teachersHavingSubject = (List<Guid>)DbContext.SubjectTeachersMappings.Where(st => st.SubjectId == subjectId).Select(t => t.TeacherId);

            if (subjectId != null)
            {
                teacher.Take(0);
                foreach (var item in teachersHavingSubject)
                {
                    teacher.Add((Teacher)teacher.Where(s => s.Id == item));
                }  
            }


            Func<Teacher, Object> orderBy = s => s.Id;
            if (OrderBy == "Id" || OrderBy == "ID" || OrderBy == "id")
            {
                orderBy = x => x.Id;
            }
            else if (OrderBy == "FullName" || OrderBy == "Name" || OrderBy == "name")
            {
                orderBy = x => x.Name;
            }
            else if (OrderBy == "Email" || OrderBy == "email")
            {
                orderBy = x => x.Email;
            }

            if (SortOrder == 1)
            {
                teacher = (DbSet<Teacher>)teacher.OrderBy(orderBy).Select(c => (c));
            }
            else
            {
                teacher = (DbSet<Teacher>)teacher.OrderByDescending(orderBy).Select(c => (c));
            }

            //pagination
            teacher = (DbSet<Teacher>)teacher.Skip((PageNumber - 1) * RecordsPerPage)
                                  .Take(RecordsPerPage);
            response.StatusCode = 200;
            response.Message = "Teacher list fetched";
            response.Data = teacher;
            return response;

        }

        public async Task<Response> UpdateTeacher(Guid Id, [FromBody] UpdateTeacher t)
        {
            Teacher? teacher = await DbContext.Teachers.FindAsync(Id);

            if (teacher != null && teacher.IsDeleted==false)
            {
                if (t.Name != "string" && t.Name != null)
                {
                    teacher.Name = t.Name;
                }
                if (t.Email != "string" && t.Email != null)
                {
                    teacher.Email = t.Email;
                }
                teacher.UpdatedAt = DateTime.UtcNow;
                await DbContext.SaveChangesAsync();

                response.StatusCode = 200;
                response.Message = "Teacher updated";
                response.Data = teacher;
                return response;
            }
            else
            {
                response.StatusCode = 404;
                response.Message = "Teacher not found";
                response.Data = string.Empty;
                return response;
            }
        }

        public async Task<Response> DeleteTeacher(Guid Id)
        {
            Teacher? teacher = await DbContext.Teachers.FindAsync(Id);

            if (teacher != null && teacher.IsDeleted==false)
            {
                teacher.IsDeleted = true;
                await DbContext.SaveChangesAsync();
                
                response.StatusCode = 200;
                response.Message = "Teacher deleted";
                response.Data = string.Empty;
                return response;
            }
            else
            {
                response.StatusCode = 404;
                response.Message = "Teacher Not found";
                response.Data = string.Empty;
                return response;
            }

        }
    }
}
