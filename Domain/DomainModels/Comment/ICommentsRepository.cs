using Domain.Comment;

namespace Domain.DomainModels.Comment
{
    public interface ICommentsRepository
    {
        public Task<Comments?> GetComment(int id);
        public void DeleteComment(Comments comment);
    }
}
