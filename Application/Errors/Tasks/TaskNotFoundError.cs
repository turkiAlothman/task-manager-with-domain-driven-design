using System.Net;

namespace Application.Errors.Tasks
{
    public record struct TaskNotFoundError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;
        public string Message => "Task not found";

    }
}
