﻿using Domain.DTOs;
using Domain.Employee;
using Domain.Project;
namespace Domain.Models.Repositories.interfaces
{
    public interface IProjectsRepository
    {
        public  Task<IEnumerable<Projects>> GetAll();
        public Task<IEnumerable<Projects>> GetWithDetails();
        public Task<Projects> GetById(int id);
        public System.Threading.Tasks.Task CreateProject(Projects project);
        public Task<IEnumerable<EmployeesDetailsWithinProjectResposne>?> GetProjectsEmployeesDetails(int  ProjectId);
        public Task<IEnumerable<ActivityRecord>> GetProjectsActivities(Projects project);
        public System.Threading.Tasks.Task RemoveEmployee(Projects project , Employees employee);
        public System.Threading.Tasks.Task AddListOfEmployeesInPoject(Projects project, List<Employees> employees);

    }
}
