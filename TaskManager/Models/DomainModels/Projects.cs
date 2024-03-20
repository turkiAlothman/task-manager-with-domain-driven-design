namespace TaskManager.Models.DomainModels
{
    public class Projects : BaseEntity
    {
        public string Name { set; get; }
        public string Type { set; get; }
        public string Description { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime DueDate { set; get; }
        public IList<Tasks> Tasks { set; get; }
        public IList<Employees> Employees { set; get; }
    }
}

