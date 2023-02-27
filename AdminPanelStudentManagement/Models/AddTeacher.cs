namespace AdminPanelStudentManagement.Models
{
    public class AddTeacher
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Subject? SubjectAllocated { get; set; }
    }
}
