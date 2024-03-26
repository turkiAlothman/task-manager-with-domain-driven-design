using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using infrastructure.Extentions;
using Domain.Models.Repositories.interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Domain.Task;
using Domain.Employee;
using Domain.Project;
using Domain.Comment;
namespace infrastructure.Persistence.Repositores
{
    public class TasksRepository : ITasksRepository
    {
        private readonly TaskManagerDbContext _Context;

        public TasksRepository(TaskManagerDbContext context)
        {
            _Context = context;
        }
        public async Task<(IEnumerable<Tasks>, int)> GetAll(int pageIndex = 1, int pageSize = 30, int? ProjectId = null, int? TeamId = null, bool AssignedToMe = false, string search = null, string Status = null, string Priority = null, int? userId = null)
        {
            var query = _Context.tasks.OrderByDescending(t => t.CreatedAt).Include(t => t.Reporter).Include(t => t.Asignees).AsQueryable();



            if (ProjectId != null)
            {
                query = query.Where(e => e.Project.Id == ProjectId);
            }
            if (TeamId != null)
            {
                query = query.Where(e => e.Asignees.Any(e => e.team.Id == TeamId));
            }
            if (AssignedToMe)
            {
                query = query.Where(e => e.Asignees.Any(e => e.Id == userId));
            }
            if (Status != null)
            {

                query = query.Where(t => t.Status == Status);

            }
            if (Priority != null)
            {
                query = query.Where(t => t.Priority == Priority);
            }


            if (!search.IsNullOrEmpty())
            {
                query = query.Where(t => t.Title.Contains(search));
            }

            int count = await query.CountAsync();

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return (await query.AsNoTracking().ToListAsync(), count);

        }


        public async Task<int> Count()
        {
            return await _Context.tasks.CountAsync();
        }

        public async Task Add(Tasks task)
        {
            await _Context.tasks.AddAsync(task);
            await _Context.SaveChangesAsync();
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
            return await _Context.tasks.Include(e => e.Asignees).Include(e => e.Reporter).Include(e => e.Project).Include(t => t.Comments).ThenInclude(c => c.Sender).Include(t => t.Activities.OrderByDescending(t => t.CreatedAt)).ThenInclude(a => a.actor).Include(e => e.Attachments).FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<Tasks> GetTaskWithAssigneesAndReporter(int id)
        {
            return await _Context.tasks.Include(t => t.Asignees).Include(t => t.Reporter).FirstOrDefaultAsync(t => t.Id == id);

        }
        public async Task<bool> IsAssigneeOrReporter(int TaskId, int EmployeeId)
        {
            return await _Context.tasks.AsNoTracking().AsQueryable().Where(t => t.Id == TaskId).Where(t => t.Asignees.Any(a => a.Id == EmployeeId) || t.Reporter.Id == EmployeeId).CountAsync() > 0;
        }
        public async Task<Tasks> GetTaskWithProject(int id)
        {
            return await _Context.tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<Tasks> GetTask(int id)
        {
            return await _Context.tasks.FirstOrDefaultAsync(t => t.Id == id);
        }
        public void AddAsignee(Tasks task, Employees Asignee)
        {
            task.Asignees.Add(Asignee);
            _Context.SaveChanges(true);
        }

        public void RemoveAsignee(Tasks task, Employees Asignee)
        {
            task.Asignees.Remove(Asignee);
            _Context.SaveChanges(true);
        }



        public void DeleteComment(Tasks task, Comments comment)
        {
            task.Comments.Remove(comment);
            _Context.SaveChanges();

        }

        public async Task<IEnumerable<Tasks>> GetByProject(Projects project)
        {
            return await _Context.tasks.AsNoTracking().Where(t => t.Project.Equals(project)).ToListAsync();
        }
    }
}
