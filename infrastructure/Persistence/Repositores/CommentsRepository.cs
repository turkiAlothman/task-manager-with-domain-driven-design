using Microsoft.EntityFrameworkCore;
using Domain.Comment;
using Domain.DomainModels.Comment;

namespace infrastructure.Persistence.Repositores
{
    public class CommentsRepository : ICommentsRepository
    {
        private TaskManagerDbContext _dbContext;
        public CommentsRepository(TaskManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Comments?> GetComment(int id)
        {
            return await _dbContext.Comments.Include(c => c.Sender).FirstOrDefaultAsync(c => c.Id == id);
        }
        public void DeleteComment(Comments comment)
        {
            _dbContext.Comments.Remove(comment);
            _dbContext.SaveChanges();
        }

    }
}
