using Microsoft.EntityFrameworkCore;
using TaskManager.Models.DomainModels;
using TaskManager.Models.Repositories.interfaces;

namespace TaskManager.Models.Repositories.implementers
{
    public class ResetPasswordRepository : IResetPasswordRepository
    {
        private readonly TaskManagerDbContext _dbContext;

        public async Task CreateResetPasswordRequest(ResetPassword resetPassword)
        {
            await _dbContext.resetPassword.AddAsync(resetPassword);
            await _dbContext.SaveChangesAsync();
        }
        
        public ResetPasswordRepository(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResetPassword> GetByEmail(string email)
        {
            return  await _dbContext.resetPassword.OrderByDescending(r=>r.CreatedAt).FirstOrDefaultAsync(r => r.Email == email);
        }
        public async Task<ResetPassword> GetByEmailAndSecret(string email, string SecretKey)
        {
            return await _dbContext.resetPassword.OrderByDescending(r => r.CreatedAt).FirstOrDefaultAsync(r => r.Email == email && r.SecretKey == SecretKey);
        }

    }
}
