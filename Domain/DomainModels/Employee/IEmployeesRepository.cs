﻿using Domain.Employee;
using Domain.Project;
using Domain.Task;

namespace Domain.DomainModels.Employee
{
    public interface IEmployeesRepository
    {
        public Task<IEnumerable<Employees>> GetAll(string search = "", int? TeamID = null, int? projectId = null);
        public Task<Employees> GetEmployee(int id);
        public Task<Employees> GetProfile(int id);
        public System.Threading.Tasks.Task CreateEmployee(Employees employee);
        public Task<Employees> GetByEmail(string email);
        public Task<IEnumerable<Employees>> Search(string SearchQuery);
        public Task<List<Employees>> getByIds(List<int> ids);

        public Task<Employees> GetByEmailAndPassword(string Email, string Password);
        public Task<IEnumerable<Tasks>> GetEmployeeTasks(Employees employee, bool Reported = false);
        public Task<IEnumerable<object>> GetEmployeeActivities(Employees employee);
        public Task<IEnumerable<object>> ExeludeEmployees(int ProjectID, string? search);
        public Task<List<Employees>> GetEmployeesByListOfIds(List<int> ids, Projects? project = null);
        public System.Threading.Tasks.Task Update(Employees employee);


    }
}
