using System.ComponentModel.DataAnnotations;

namespace AdminPanelStudentManagement.Models
{
    public class UpdateStudent
    {
        public string Name { get; set; } = "string";

        [StringLength(256)]
        [EmailAddress]
        public string Email { get; set; } = "string";
        public List<SubjectTeacherMappings>? SubjectTeacherAllocated { get; set; } = new List<SubjectTeacherMappings>();
    }
}
