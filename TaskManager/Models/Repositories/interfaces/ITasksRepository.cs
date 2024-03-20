using TaskManager.Models.DomainModels;
using TaskManager.RequestForms;

namespace TaskManager.Models.Repositories.interfaces
{
    public interface ITasksRepository
    {
        public Task<(IEnumerable<Tasks>, int)> GetAll(TasksFilterQuery queryParameters);
        public Task<int> Count();
        public Task Add(Tasks task);
        public Task Delete(Tasks task);
        public Task<Tasks> GetTaskWithAllDetails(int id);
        public Task<Tasks> GetTaskWithAssigneesAndReporter(int id);
        public Task<bool> IsAssigneeOrReporter(int TaskId, int EmployeeId);
        public Task<Tasks> GetTaskWithProject(int id);
        public Task<Tasks> GetTask(int id);
        
        public void Update(Tasks task);
        public void AddAsignee(Tasks task, Employees Asignee);

        public void RemoveAsignee(Tasks task, Employees Asignee);

        public void AddComment(Tasks task , Comments comment);

        public Task<IEnumerable<Tasks>> GetByProject(Projects project);
        

    }
}
