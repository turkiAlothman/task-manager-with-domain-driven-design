using Application.Errors;
using Domain.Task;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Services.Interfaces
{
    public interface ITasksService
    {
        public Task<IError> Create(int UserId, string title, string description, List<int> asignees, int project, string priority, string type, DateTime startDate, DateTime deadline, List<IFormFile> Attachments);
        public Task<IError> Update(int UserId, int TaskID, JsonPatchDocument<Tasks> patch);
        public Task<IError> Delete(int UserId, int TaskID);
        public Task<IError> AddAsignee(int UserId, int AssigneeId, int TaskId);
        public Task<IError> RemoveAsignee(int UserId, int AssigneeId, int TaskId);
        public Task<IError> AddComment(int UserId, int TaskId, string MessageContent);
        public Task<IError> DeleteComment(int UserId, int CommentID);

    }
}
