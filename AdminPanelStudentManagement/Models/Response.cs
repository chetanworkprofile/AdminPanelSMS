namespace AdminPanelStudentManagement.Models
{
    public class Response
    {
        public int StatusCode { get; set; } = 200;
        public string Message { get; set; } = "Ok";
        public Object Data { get; set; } = new Object();
    }
}
