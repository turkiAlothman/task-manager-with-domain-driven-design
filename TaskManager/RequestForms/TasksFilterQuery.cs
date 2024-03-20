namespace TaskManager.RequestForms
{
    public class TasksFilterQuery
    {
        public int pageIndex { set; get; } = 1;
        public int pageSize { set; get; } = 30;
        public int? ProjectId { set; get; } = null;
        public int? TeamId { set; get; } = null;
        public bool AssignedToMe { set; get; } = false;
        public string search { set; get; } = null;
        public string Status { set; get; } = null;
        public string Priority { set; get; } = null;
    }
}
