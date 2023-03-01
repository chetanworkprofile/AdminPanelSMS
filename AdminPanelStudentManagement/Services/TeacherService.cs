using AdminPanelStudentManagement.Data;
using AdminPanelStudentManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
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
            bool tempTeacher = DbContext.Teachers.Where(t=>t.Email==addTeacher.Email).Any();
            if(tempTeacher == false)
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
                    //SubjectsAllocated = new List<Subject>(),
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
            response.Data = string.Empty;
            return response;
        }

        public Response GetTeachers(Guid? TeacherID, string? Name, string? Email, Guid? subjectId, String OrderBy, int SortOrder, int RecordsPerPage, int PageNumber)          // sort order   ===   e1 for ascending   -1 for descending
        {
            var teacher = DbContext.Teachers.Where(t => t.IsDeleted == false).ToList();
            if (TeacherID != null) { teacher = teacher.Where(s => (s.Id == TeacherID)).ToList(); }
            if (Name != null) { teacher = teacher.Where(s => (s.Name == Name)).ToList(); }
            if (Email != null) { teacher = teacher.Where(s => (s.Email == Email)).ToList(); }
            List<Guid> teachersHavingSubject = DbContext.SubjectTeachersMappings.Where(st => st.SubjectId == subjectId).Select(t => t.TeacherId).ToList();

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
                teacher = teacher.OrderBy(orderBy).Select(c => (c)).ToList();
            }
            else
            {
                teacher = teacher.OrderByDescending(orderBy).Select(c => (c)).ToList();
            }

            //pagination
            teacher = teacher.Skip((PageNumber - 1) * RecordsPerPage)
                                  .Take(RecordsPerPage).ToList();
            List<TeacherResponse> teacherResponses= new List<TeacherResponse>();
            foreach(var a in teacher)
            {
                TeacherResponse t = new TeacherResponse()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Email = a.Email,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                    //SubjectsAllocated = DbContext.Subjects.Where(s => s.Id == DbContext.SubjectTeachersMappings.Where(st => st.TeacherId == a.Id).Select(s => s.SubjectId))
                };
                teacherResponses.Add(t);
            }
            response.StatusCode = 200;
            response.Message = "Teacher list fetched";
            response.Data = teacherResponses;
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
