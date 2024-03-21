using Domain.Entities;

namespace Domain.Models.Repositories.interfaces
{
    public interface ICommentsRepository
    {
        public Task<Comments?> GetComment(int id);
        public void DeleteComment(Comments comment);
    }
}
