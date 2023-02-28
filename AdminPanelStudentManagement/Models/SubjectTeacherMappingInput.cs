using System.ComponentModel.DataAnnotations;

namespace AdminPanelStudentManagement.Models
{
    public class SubjectTeacherMappingInput
    {
        [Required]
        public Guid TeacherId { get; set; }
        [Required]
        public Guid SubjectId { get; set; }
    }
}
