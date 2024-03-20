using Microsoft.EntityFrameworkCore;
using System.Linq;
using TaskManager.ExtentionsMethods;
using TaskManager.Models.DomainModels;
using TaskManager.Models.Repositories.interfaces;

namespace TaskManager.Models.Repositories.implementers
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly TaskManagerDbContext _context;
        public EmployeesRepository(TaskManagerDbContext context) { 
        this._context = context;
        }

        public async Task<IEnumerable<Employees>> GetAll(string search="", int? TeamID = null , Projects? project = null)
        {

            var query =  _context.employees.AsNoTracking().OrderBy(e=>e.Id).Include(e=>e.team).Where(e => e.FirstName.Contains(search) || e.LastName.Contains(search)).AsQueryable();
            
            if(TeamID != null)
                query = query.Where(e => e.team.Id == TeamID);
            
            if(project != null)
                query = query.Where(e => e.Projects.Contains(project));

            return await query.ToListAsync(); ;

        }
        public async Task<Employees> GetEmployee(int id) {
            return await this._context.employees.FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<Employees> GetProfile(int id)
        {
            return await _context.employees.Include(e => e.Projects).ThenInclude(p => p.Employees).Include(t => t.team).ThenInclude(t => t.Members).FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task CreateEmployee(Employees employee)
        {
            await _context.employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<Employees> GetByEmail(string email)
        {
            return await _context.employees.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<IEnumerable<Employees>> Search(string SearchQuery) { 
        
            return await this._context.employees.Where(e => e.FirstName.Contains(SearchQuery) || e.LastName.Contains( SearchQuery)).Include(e=>e.team).ToListAsync();
        }

        public async Task< List<Employees>> getByIds(List<int> ids)
        {

           return await _context.employees.Where(e => ids.Contains(e.Id)).ToListAsync();
        }

        public async Task<Employees> GetByEmailAndPassword(string Email, string Password)
        {
            return await this._context.employees.FirstOrDefaultAsync(e => e.Email == Email && e.Password == Password.Hash());
        }

        public async Task<IEnumerable<Tasks>> GetEmployeeTasks(Employees employee, bool Reported = false)
        {
            var query = _context.employees.AsNoTracking().Where(e => e.Equals(employee));


            return  (!Reported)? await query.Select(e => e.Tasks).FirstAsync() : await query.Select(e => e.tasksReported).FirstAsync();
        }
        public async Task<IEnumerable< Object>> GetEmployeeActivities(Employees employee)
        {
            return await _context.employees.AsNoTracking().Where(e=>e.Equals(employee)).Select(e=>e.activities.Select( 
                a=> new {
                description = a.description,
                actor = new { FirstName = a.actor.FirstName, LastName = a.actor.LastName, Position = a.actor.Position, Email = a.actor.Email, PhoneNumber = a.actor.PhoneNumber, },
                task = (a.task !=null)? new { Id = a.task.Id, Title = a.task.Title }:null,
                ProjectName = a.ProjectName,
                CreatedAt = a.CreatedAt

            })).FirstAsync();
        }

        public async Task<IEnumerable<Object>> ExeludeEmployees(int ProjectID, string? search)
        {
            var Query = _context.employees
                .Where(e => !e.Projects.Select(p => p.Id).Contains(ProjectID));

                if(search != null)
                Query = Query.Where(e=>e.FirstName.Contains(search) || e.LastName.Contains(search));

            

            return await Query.Select(e => new
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Position = e.Position,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
            }).ToListAsync();
        }


        public async Task<List<Employees>> GetEmployeesByListOfIds(List<int> ids,Projects? project = null)
        {
            var query = _context.employees.Include(e=>e.Projects).Where(e => ids.Contains(e.Id));

            if (project != null)
                query.Where(e => e.Projects.Contains(project));

            return await query.Select(e=>e).ToListAsync();
        }

        public async Task Update(Employees employee)
        {
             _context.employees.Update(employee);
            await _context.SaveChangesAsync();

        }

    }
}
