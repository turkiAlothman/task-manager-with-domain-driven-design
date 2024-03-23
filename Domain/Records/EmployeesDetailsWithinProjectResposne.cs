namespace Domain.Records
{
    public class EmployeesDetailsWithinProjectResposne
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string team { get; set; }
        public int SignedTasksCount { get; set; }
        public int ReportedTasksCount { get; set; }
    }
}
