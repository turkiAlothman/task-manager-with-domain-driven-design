using Domain.Base;
using infrastructure.Persistence;

namespace infrastructure
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly TaskManagerDbContext _dbContext;

        public UnitOfWork(TaskManagerDbContext dbContext) {  _dbContext = dbContext; }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
