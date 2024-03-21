namespace Domain.Models.DomainModels
{
    public class Activities : BaseEntity
    {
        public Tasks? task { get; set; }
        public Employees actor { get; set; }
        public string description { get; set; }
        public string ProjectName {get; set; }
    }
}
