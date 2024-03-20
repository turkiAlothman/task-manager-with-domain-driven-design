using TaskManager.Models.DomainModels;

namespace TaskManager.Models.Repositories.interfaces
{
    public interface ICommentsRepository
    {
        public Task<Comments?> GetComment(int id);
        public void DeleteComment(Comments comment);
    }
}
