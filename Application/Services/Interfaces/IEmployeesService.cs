using Application.Errors;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IEmployeesService
    {
        public Task<IEnumerable<Employees>> Search(string search = "", int? projectId = null);
        public Task<IEnumerable<Object>> ExeludeEmployees(int ProjectId,string? search = null);
        public Task<OneOf<IEnumerable<Tasks>, IError>> GetEmployeeTasks(int id, bool Reported = false);
        public Task<OneOf<IEnumerable<Object>, IError>> GetEmployeeActivities(int id);
        
    }
}
