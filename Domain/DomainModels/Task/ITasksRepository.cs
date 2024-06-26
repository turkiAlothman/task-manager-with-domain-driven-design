﻿using Domain.DTOs;
using Domain.Employee;
using Domain.Project;
using Domain.Task;


namespace Domain.DomainModels.Task
{
    public interface ITasksRepository
    {
        public Task<(IEnumerable<Tasks>, int)> GetAll(int pageIndex = 1, int pageSize = 30, int? ProjectId = null, int? TeamId = null, bool AssignedToMe = false, string search = null, string Status = null, string Priority = null, int? userId = null);
        public Task<int> Count();
        public System.Threading.Tasks.Task Add(Tasks task);
        public System.Threading.Tasks.Task Delete(Tasks task);
        public Task<Tasks> GetTaskWithAllDetails(int id);
        public Task<Tasks> GetTaskWithAssigneesAndReporter(int id);
        public Task<bool> IsAssigneeOrReporter(int TaskId, int EmployeeId);
        public Task<Tasks> GetTaskWithProject(int id);
        public Task<Tasks> GetTask(int id);

        public void Update(Tasks task);
        public void AddAsignee(Tasks task, Employees Asignee);

        public void RemoveAsignee(Tasks task, Employees Asignee);

        public Task<IEnumerable<Tasks>> GetByProject(Projects project);
        public Task<List<PriorityStatus>> GetPriorityStatus();
        public Task<List<TasksStatusPercentage>> GetTasksStatusPercentage();


    }
}
