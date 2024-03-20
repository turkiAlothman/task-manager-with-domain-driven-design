using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using TaskManager.Extentions;
using TaskManager.Models.DomainModels;
using TaskManager.Models.Repositories.interfaces;
using TaskManager.RequestForms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace TaskManager.Models.Repositories.implementers
{
    public class TasksRepository : ITasksRepository
    {
        private readonly TaskManagerDbContext _Context;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public TasksRepository(TaskManagerDbContext context, IHttpContextAccessor HttpContextAccessor)
        {
            _Context = context;
            _HttpContextAccessor = HttpContextAccessor;
        } 
        public async Task<(IEnumerable<Tasks>,int)> GetAll(TasksFilterQuery queryParameters) {
            var query =  _Context.tasks.OrderByDescending(t => t.CreatedAt).Include(t=>t.Reporter).Include(t=>t.Asignees).AsQueryable();

            

            if (queryParameters.ProjectId != null)
            {
                query = query.Where(e => e.Project.Id == queryParameters.ProjectId);
            }
            if (queryParameters.TeamId != null)
            {
                query = query.Where(e => e.Asignees.Any(e=>e.team.Id == queryParameters.TeamId));
            }
            if (queryParameters.AssignedToMe)
            {
                query = query.Where(e => e.Asignees.Any(e => e.Id == _HttpContextAccessor.GetUserID()));
            }
            if(queryParameters.Status != null) { 
            
                query = query.Where(t=>t.Status  == queryParameters.Status);
            
            }
            if (queryParameters.Priority != null)
            {
                query = query.Where(t => t.Priority == queryParameters.Priority);
            }


            if (!queryParameters.search.IsNullOrEmpty())
            {
                query = query.Where(t => t.Title.Contains(queryParameters.search));
            }

            int count = await query.CountAsync();

            query = query.Skip((queryParameters.pageIndex - 1) * queryParameters.pageSize).Take(queryParameters.pageSize);

            return (await query.AsNoTracking().ToListAsync() , count);

        }


        public async Task<int> Count()
        {
            return await _Context.tasks.CountAsync();  
        }

        public async Task Add(Tasks task)
        {
            await _Context.tasks.AddAsync(task);
            _Context.SaveChanges();
        }

        public async Task Delete(Tasks task)
        {
             _Context.tasks.Remove(task);

            await _Context.SaveChangesAsync();
        }

        public void Update(Tasks task)
        {
            _Context.tasks.Update(task);
            _Context.SaveChanges();

        }

        public async Task<Tasks> GetTaskWithAllDetails(int id)
        {
            return await _Context.tasks.Include(e => e.Asignees).Include(e => e.Reporter).Include(e => e.Project).Include(t=>t.Comments).ThenInclude(c=>c.Sender).Include(t=>t.Activities.OrderByDescending(t => t.CreatedAt)).ThenInclude(a=>a.actor).Include(e=>e.Attachments).FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<Tasks> GetTaskWithAssigneesAndReporter(int id)
        {
            return await _Context.tasks.Include(t => t.Asignees).Include(t=>t.Reporter).FirstOrDefaultAsync(t=>t.Id == id);
                
        }
        public async Task<bool> IsAssigneeOrReporter(int TaskId ,  int EmployeeId)
        {
            return await _Context.tasks.AsNoTracking().AsQueryable().Where(t=>t.Id == TaskId).Where(t=>t.Asignees.Any(a=>a.Id == EmployeeId) || t.Reporter.Id == EmployeeId).CountAsync() > 0;
        }
        public async Task<Tasks> GetTaskWithProject(int id)
        {
            return await _Context.tasks.Include(t => t.Project).FirstOrDefaultAsync(t=>t.Id==id);
        }
        public async Task<Tasks> GetTask(int id)
        {
            return await _Context.tasks.FirstOrDefaultAsync(t=>t.Id == id);
        }
        public void AddAsignee(Tasks task, Employees Asignee)
        {
            task.Asignees.Add(Asignee);
            this._Context.SaveChanges(true);
        }

        public void RemoveAsignee(Tasks task, Employees Asignee)
        {
            task.Asignees.Remove(Asignee);
            this._Context.SaveChanges(true);
        }

        public void AddComment(Tasks task, Comments comment)
        {
            task.Comments = new List<Comments>();
            task.Comments.Add(comment);
            this._Context.SaveChanges();
            
        }

        public void DeleteComment(Tasks task, Comments comment)
        {
            task.Comments.Remove(comment);
            this._Context.SaveChanges();

        }

        public async Task<IEnumerable<Tasks>> GetByProject(Projects project)
        {
            return await _Context.tasks.AsNoTracking().Where(t => t.Project.Equals(project)).ToListAsync();
        }
    }
}
