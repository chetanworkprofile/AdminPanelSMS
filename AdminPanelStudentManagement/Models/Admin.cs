using System.ComponentModel.DataAnnotations;

namespace AdminPanelStudentManagement.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; } = 1;
        public string? Username { get; set; } = "Admin";
        public string? Password { get; set; } = "admin123";
    }
}
