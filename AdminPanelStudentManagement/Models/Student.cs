using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AdminPanelStudentManagement.Models
{
    public class Student
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public List<SubjectTeacherMappings>? SubjectTeacherAllocated { get; set; }

        //public string PathToProfilePic { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
