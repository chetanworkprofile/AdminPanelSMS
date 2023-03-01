using AdminPanelStudentManagement.Data;
using AdminPanelStudentManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystemAPI.Models;
using System.Linq;

namespace AdminPanelStudentManagement.Services
{
    public class SubjectService: ISubjectService
    {
        private readonly AdminSMSAPIDbContext DbContext;
        AuthService _authService;
        Response response = new Response();
        public SubjectService(AdminSMSAPIDbContext dbContext, IConfiguration configuration)
        {
            DbContext = dbContext;
            _authService = new AuthService(dbContext, configuration);
        }
        public async Task<Response> CreateSubject(AddSubject addSubject)
        {
            Subject? subjectExist = await DbContext.Subjects.FindAsync(addSubject);
            //int subjectCount = await DbContext.Subjects.CountAsync();
            if (subjectExist == null)
            {
                var Subject = new Subject()
                {
                    Id = new Guid(),
                    Name = addSubject.Name,
                    Description = addSubject.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false
                };
                SubjectResponse subjectResponse= new SubjectResponse()
                {
                    Id = Subject.Id,
                    Name = Subject.Name,
                    Description = Subject.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                await DbContext.Subjects.AddAsync(Subject);
                await DbContext.SaveChangesAsync();
                response.StatusCode = 200;
                response.Message = "Subject created";
                response.Data = subjectResponse;
                return response;
            }
            response.StatusCode = 409;
            response.Message = "Subject Already Exists";
            response.Data = subjectExist;
            return response;
        }

        public Response GetSubjects(Guid? subjectId, string? Name, String OrderBy, int SortOrder, int RecordsPerPage, int PageNumber)          // sort order   ===   e1 for ascending   -1 for descending
        {
            var subject = DbContext.Subjects;
            subject = (DbSet<Subject>)subject.Where(t => t.IsDeleted == false);
            if (subjectId != null) { subject = (DbSet<Subject>)subject.Where(s => (s.Id == subjectId)); }
            if (Name != null) { subject = (DbSet<Subject>)subject.Where(s => (s.Name == Name)); }


            Func<Subject, Object> orderBy = s => s.Id;
            if (OrderBy == "Id" || OrderBy == "ID" || OrderBy == "id")
            {
                orderBy = x => x.Id;
            }
            else if (OrderBy == "FullName" || OrderBy == "Name" || OrderBy == "name")
            {
                orderBy = x => x.Name;
            }

            if (SortOrder == 1)
            {
                subject = (DbSet<Subject>)subject.Select(c => (c));
            }
            else
            {
                subject = (DbSet<Subject>)subject.Select(c => (c));
            }

            //pagination
            subject = (DbSet<Subject>)subject.Skip((PageNumber - 1) * RecordsPerPage)
                                  .Take(RecordsPerPage);
            response.StatusCode = 200;
            response.Message = "Subject list fetched";
            response.Data = subject;
            return response;
        }

        public async Task<Response> UpdateSubject(Guid Id, UpdateSubject s)
        {
            Subject? subject = await DbContext.Subjects.FindAsync(Id);

            if (subject != null && subject.IsDeleted == false)
            {
                if (s.Name != "string" && s.Name != null)
                {
                    subject.Name = s.Name;
                }
                if (s.Description != "string" && s.Description != null)
                {
                    subject.Description = s.Description;
                }
                subject.UpdatedAt = DateTime.UtcNow;
                await DbContext.SaveChangesAsync();

                response.StatusCode = 200;
                response.Message = "Subject updated";
                response.Data = subject;
                return response;
            }
            else
            {
                response.StatusCode = 404;
                response.Message = "Subject not found";
                response.Data = new Teacher();
                return response;
            }
        }

        public async Task<Response> DeleteSubject(Guid Id)
        {
            Subject? subject = await DbContext.Subjects.FindAsync(Id);

            if (subject != null && subject.IsDeleted == false)
            {
                subject.IsDeleted = true;
                List<SubjectTeacherMappings> subjectTeacherMapping = (List<SubjectTeacherMappings>)DbContext.SubjectTeachersMappings.Where(t => t.SubjectId == Id);
                foreach(SubjectTeacherMappings st in subjectTeacherMapping)
                {
                    List<Student> students = (List<Student>)DbContext.Students.Where(s => s.SubjectTeacherAllocated.Contains(st));
                    foreach(Student s in students)
                    {
                        DbContext.Students.Find(s).SubjectTeacherAllocated.Remove(st);
                    }
                    DbContext.SubjectTeachersMappings.Remove(st);
                }

                //List<Student> students = (List<Student>)DbContext.Students.Where(s => s.SubjectTeacherAllocated.Contains(subjectTeacherMapping));
                /*foreach(Student s in students)
                {
                    s.SubjectsAllocated.Remove(subject);
                }*/
                await DbContext.SaveChangesAsync();

                response.StatusCode = 200;
                response.Message = "Subject deleted";
                response.Data = string.Empty;
                return response;
            }
            else
            {
                response.StatusCode = 404;
                response.Message = "Subject Not found";
                response.Data = string.Empty;
                return response;
            }

        }
    }
}
