namespace AdminPanelStudentManagement.Models
{
    public class TeacherResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        //public List<Subject>? SubjectsAllocated { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
