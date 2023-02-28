using System.ComponentModel.DataAnnotations;

namespace AdminPanelStudentManagement.Models
{
    public class AddSubject
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
