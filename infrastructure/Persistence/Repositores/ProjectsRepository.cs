using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Linq;
using System.Collections.Generic;
using Domain.DTOs;
using Org.BouncyCastle.Crypto;
using Domain.Employee;
using Domain.Project;
using Domain.DomainModels.ResetPasswords;


namespace infrastructure.Persistence.Repositores
{
    public class ProjectsRepository : IProjectsRepository 
    {
        private readonly TaskManagerDbContext _context;

        public ProjectsRepository(TaskManagerDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<IEnumerable<Projects>> GetAll()
        {
            return await _context.projects.Include(p => p.Employees).ToListAsync();
        }
        public async Task<IEnumerable<Projects>> GetWithDetails()
        {
            return await _context.projects.AsNoTracking().Include(p => p.Employees).ThenInclude(e => e.team).ToListAsync();
        }
        public async Task<Projects> GetById(int id)
        {
            return await _context.projects.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<int> Count()
        {
            return await _context.projects.CountAsync();
        }
        public async Task CreateProject(Projects project)
        {
            _context.projects.Add(project);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<EmployeesDetailsWithinProjectResposne>?> GetProjectsEmployeesDetails(int ProjectId)
        {
            

                return await _context.projects
                .AsNoTracking().AsQueryable().Where(p => p.Id == ProjectId)
                .Select(p => p.Employees.Select(e => new
               EmployeesDetailsWithinProjectResposne()
                {
                    id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Position = e.Position,
                    PhoneNumber = e.PhoneNumber,
                    Email = e.Email,
                    team = e.team.Name,
                    SignedTasksCount = e.Tasks.Where(t => t.Project.Id == ProjectId).Count(),
                    ReportedTasksCount = e.tasksReported.Where(t => t.Project.Id == ProjectId).Count(),
                })).FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<ActivityRecord>> GetProjectsActivities(Projects project)
        {
            return await _context.projects
                .AsNoTracking()
                .Where(p => p.Equals(project))
                .Select(p => p.Tasks.SelectMany(t => t.Activities
                .Select(a =>
                new ActivityRecord()
                {
                    description = a.description,
                    actor = new ActivityActorRecord{ FirstName = a.actor.FirstName, LastName = a.actor.LastName, Position = a.actor.Position, Email = a.actor.Email, PhoneNumber = a.actor.PhoneNumber, },
                    task = new ActivityTaskRecord{ Id = a.task.Id, Title  = a.task.Title },
                    ProjectName = a.ProjectName,
                    CreatedAt = a.CreatedAt

                }))).FirstAsync();
        }


        public async Task RemoveEmployee(Projects project, Employees employee)
        {
            string sqlQuery = "DELETE FROM EmployeesProjects WHERE EmployeesId = @EmployeesId AND ProjectsId = @ProjectsId";
            await _context.Database.ExecuteSqlRawAsync(sqlQuery, [new MySqlParameter("@EmployeesId", employee.Id), new MySqlParameter("@ProjectsId", project.Id)]);
        }


        
    }
}
