namespace AdminPanelStudentManagement.Models
{
    public class StudentResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Dictionary<Subject, Teacher>? SubjectTeacherPair { get; set; }

        //public string PathToProfilePic { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
