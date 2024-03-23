using Application.Errors;
using Domain.Entities;
using Domain.Records;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using System.ComponentModel.DataAnnotations;

namespace Application.Services.Interfaces
{
    public interface IProjectsService
    {
        public Task<OneOf<IEnumerable<EmployeesDetailsWithinProjectResposne>,IError>> GetProjectsEmployeesDetails(int projectsId);
        public Task<IError> CreateProject(bool IsManager , string Name, string Type, string Description, DateTime StartDate, DateTime DueDate);
        public Task<OneOf<IEnumerable<Tasks>,IError>> GetProjectsTasksDetails(int ProjectId);
        public Task<OneOf<IEnumerable<ActivityRecord>, IError>> GetProjectsActivities(int ProjectId);
        public Task<IError> AddEmployeeToProject(bool IsManager, int ProjectId,List<int> EmployeesIds);
        public Task<IError> DeleteEmployee(bool IsManager, int ProjectId, int employeeId);


    }
}
