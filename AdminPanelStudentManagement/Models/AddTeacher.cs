using System.ComponentModel.DataAnnotations;
using AdminPanelStudentManagement.Validations;

namespace AdminPanelStudentManagement.Models
{
    public class AddTeacher
    {
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string? Name { get; set; }
        [Required]
        [StringLength(256)]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Passord")]
        public string? Password { get; set; }
        //[SubjectValidation]
    }
}
