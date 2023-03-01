using AdminPanelStudentManagement.Data;
using AdminPanelStudentManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystemAPI.Models;
using System.Linq;

namespace AdminPanelStudentManagement.Services
{
    public class StudentService:IStudentService
    {
        private readonly AdminSMSAPIDbContext DbContext;
        AuthService _authService;
        Response response = new Response();
        public StudentService(AdminSMSAPIDbContext dbContext, IConfiguration configuration)
        {
            DbContext = dbContext;
            _authService = new AuthService(dbContext, configuration);
        }
        public async Task<Response> CreateStudent(AddStudent addStudent)
        {
            Student? tempStudent = await DbContext.Students.FindAsync(addStudent.Email);
            /*List<Subject> allocateSubject = await DbContext.Subjects.Where(s=>s.Name);*/
            List<SubjectTeacherMappings> subjects = new List<SubjectTeacherMappings>();
            
            if (tempStudent == null)
            {
                foreach(var a in addStudent.SubjectsAllocated)
                {
                     subjects.Add((SubjectTeacherMappings)DbContext.SubjectTeachersMappings.Where(s => s.SubjectId == a.SubjectId && s.TeacherId == a.TeacherId));
                }
                var student = new Student()
                {
                    Id = Guid.NewGuid(),
                    Name = addStudent.Name,
                    Email = addStudent.Email,
                    PasswordHash = _authService.CreatePasswordHash(addStudent.Password),
                    SubjectTeacherAllocated = subjects,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false
                };
                StudentResponse studentResponse = new StudentResponse()
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    SubjectTeacherAllocated = student.SubjectTeacherAllocated,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                await DbContext.Students.AddAsync(student);
                await DbContext.SaveChangesAsync();
                response.StatusCode = 200;
                response.Message = "Student created";
                response.Data = studentResponse;
                return response;
            }
            response.StatusCode = 409;
            response.Message = "Email already registered";
            response.Data = tempStudent;
            return response;
        }

        public Response GetStudents(Guid? StudentId, string? Name, string? Email, Guid? subjectId, String OrderBy, int SortOrder, int RecordsPerPage, int PageNumber)          // sort order   ===   e1 for ascending   -1 for descending
        {
            var student = DbContext.Students;
            student = (DbSet<Student>)student.Where(t => t.IsDeleted == false);
            if (StudentId != null) { student = (DbSet<Student>)student.Where(s => (s.Id == StudentId)); }
            if (Name != null) { student = (DbSet<Student>)student.Where(s => (s.Name == Name)); }
            if (Email != null) { student = (DbSet<Student>)student.Where(s => (s.Email == Email)); }
            
            /*if (subjectId != null)
            {
                List<SubjectTeacherMappingInput> subjectTeacherId = (List<SubjectTeacherMappingInput>)DbContext.SubjectTeachersMappings.Where(st => st.SubjectId == subjectId).Select(st => st);
                //SubjectTeacherMappings temp = DbContext.SubjectTeachersMappings.Where(s => s.SubjectId.(subjectId));
                student = (DbSet<Student>)student.Where(s => s.SubjectTeacherAllocated.));
            }*/


            Func<Student, Object> orderBy = s => s.Id;
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
                student = (DbSet<Student>)student.OrderBy(orderBy).Select(c => (c));
            }
            else
            {
                student = (DbSet<Student>)student.OrderByDescending(orderBy).Select(c => (c));
            }

            //pagination
            student = (DbSet<Student>)student.Skip((PageNumber - 1) * RecordsPerPage)
                                  .Take(RecordsPerPage);

            response.StatusCode = 200;
            response.Message = "Student list fetched";
            response.Data = student;
            return response;
        }

        public async Task<Response> UpdateStudent(Guid Id, UpdateStudent s)
        {
            Student? student = await DbContext.Students.FindAsync(Id);

            if (student != null && student.IsDeleted == false)
            {
                if (s.Name != "string" && s.Name != null)
                {
                    student.Name = s.Name;
                }
                if (s.Email != "string" && s.Email != null)
                {
                    student.Email = s.Email;
                }

                /*List<Subject> subjects = new List<Subject>();
                if (s.SubjectsAllocated != subjects)
                {
                    student.SubjectsAllocated = s.SubjectsAllocated;
                }
                student.UpdatedAt = DateTime.UtcNow;
                await DbContext.SaveChangesAsync();*/

                response.StatusCode = 200;
                response.Message = "Student updated";
                response.Data = student;
                return response;
            }
            else
            {
                response.StatusCode = 404;
                response.Message = "Student not found";
                response.Data = string.Empty;
                return response;
            }
        }

        public async Task<Response> DeleteStudent(Guid Id)
        {
            Student? student= await DbContext.Students.FindAsync(Id);

            if (student != null && student.IsDeleted == false)
            {
                student.IsDeleted = true;
                await DbContext.SaveChangesAsync();

                response.StatusCode = 200;
                response.Message = "Student deleted";
                response.Data = string.Empty;
                return response;
            }
            else
            {
                response.StatusCode = 404;
                response.Message = "Student Not found";
                response.Data = string.Empty;
                return response;
            }

        }
    }
}
