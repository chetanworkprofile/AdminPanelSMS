namespace AdminPanelStudentManagement.Models
{
    public class Teacher
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public Subject? SubjectAllocated { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
