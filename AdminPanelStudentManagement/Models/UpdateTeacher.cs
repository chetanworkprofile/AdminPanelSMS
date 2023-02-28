using AdminPanelStudentManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystemAPI.Models
{
    public class UpdateTeacher
    {
        public string Name { get; set; } = "string";

        [StringLength(256)]
        [EmailAddress]
        public string Email { get; set; } = "string";

        //[Url]
        //public string PathToProfilePic { get; set; } = new PathString();
    }
}
