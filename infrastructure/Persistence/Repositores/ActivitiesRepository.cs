using Microsoft.EntityFrameworkCore;
using Domain.Models.DomainModels;
using Domain.Models.Repositories.interfaces;

namespace infrastructure.Persistence.Repositores
{
    public class ActivitiesRepository : IActivitiesRepository
    {
        private readonly TaskManagerDbContext _dbContext;
        public ActivitiesRepository(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async void AddChangeStatusActivities(List<Activities> activites)
        {
            await _dbContext.Activities.AddRangeAsync(activites);
            _dbContext.SaveChanges();

        }

        public async Task<IEnumerable<Activities>> GetAllActivities(int pageIndex = 1, int pageSize = 60)
        {
            var query = _dbContext.Activities.Include(a => a.task).Include(a => a.actor).OrderByDescending(a => a.CreatedAt).AsQueryable();

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<int> count()
        {
            return await _dbContext.Activities.CountAsync();
        }
    }
}
