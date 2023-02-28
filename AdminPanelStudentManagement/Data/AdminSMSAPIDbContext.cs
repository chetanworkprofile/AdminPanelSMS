using AdminPanelStudentManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminPanelStudentManagement.Data
{
    public class AdminSMSAPIDbContext : DbContext
    {
        public AdminSMSAPIDbContext(DbContextOptions options): base(options)
        {
        }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectTeacherMappings> SubjectTeachersMappings { get;set; }
    }
}
