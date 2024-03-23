namespace Domain.Records
{
    public record ActivityRecord
    {
        public string description { get; set; }
        public ActivityActorRecord actor { get; set; }
        public ActivityTaskRecord task { get; set; }

        public string ProjectName { get; set; }
        public DateTime CreatedAt { set; get; }

    }


    public record ActivityActorRecord
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }


    public record ActivityTaskRecord
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
