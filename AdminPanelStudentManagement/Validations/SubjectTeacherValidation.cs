using AdminPanelStudentManagement.Data;
using System.ComponentModel.DataAnnotations;

namespace AdminPanelStudentManagement.Validations
{
    public class SubjectTeacherValidation : ValidationAttribute
    {
        private readonly AdminSMSAPIDbContext _dbContext;
        public SubjectTeacherValidation(AdminSMSAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            Console.Write(value);
            bool subjectExists = _dbContext.Subjects.Contains(value);

            if (!subjectExists)
            {
                return new ValidationResult("Subject Does Not Exists");
            }
            return ValidationResult.Success;
        }
    }
}
//do validation later for student teacher mappings so no stuent should get a subject twice and wrong subject-teacher pair