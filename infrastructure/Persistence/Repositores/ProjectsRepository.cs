using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Linq;
using Domain.Models.Repositories.interfaces;
using System.Collections.Generic;
using Domain.Entities;


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

        public async Task CreateProject(Projects project)
        {
            _context.projects.Add(project);
            await _context.SaveChangesAsync();
        }
        public async Task<object?> GetProjectsEmployeesDetails(Projects project)
        {
            return await _context.projects
                .AsNoTracking().Where(p => p.Id == project.Id)
                .Select(p => p.Employees.Select(e => new
                {
                    id = e.Id,
                    e.FirstName,
                    e.LastName,
                    e.Position,
                    e.PhoneNumber,
                    e.Email,
                    team = e.team.Name,
                    SignedTasksCount = e.Tasks.Where(t => t.Project.Id == project.Id).Count(),
                    ReportedTasksCount = e.tasksReported.Where(t => t.Project.Id == project.Id).Count(),
                })).FirstAsync();
        }


        public async Task<IEnumerable<object>> GetProjectsActivities(Projects project)
        {
            return await _context.projects
                .AsNoTracking()
                .Where(p => p.Equals(project))
                .Select(p => p.Tasks.SelectMany(t => t.Activities
                .Select(a =>
                new
                {
                    a.description,
                    actor = new { a.actor.FirstName, a.actor.LastName, a.actor.Position, a.actor.Email, a.actor.PhoneNumber, },
                    task = new { a.task.Id, a.task.Title },
                    a.ProjectName,
                    a.CreatedAt

                }))).FirstAsync();
        }


        public async Task RemoveEmployee(Projects project, Employees employee)
        {
            string sqlQuery = "DELETE FROM EmployeesProjects WHERE EmployeesId = @EmployeesId AND ProjectsId = @ProjectsId";
            await _context.Database.ExecuteSqlRawAsync(sqlQuery, [new MySqlParameter("@EmployeesId", employee.Id), new MySqlParameter("@ProjectsId", project.Id)]);
        }


        public async Task AddListOfEmployeesInPoject(Projects project, List<Employees> employees)
        {
            project.Employees = new List<Employees>();
            foreach (Employees emmployee in employees)
                project.Employees.Add(emmployee);

            await _context.SaveChangesAsync();




        }
    }
}
