using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors.Comments
{
    public record struct CommentNotFoundError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;
        public string Message => "Comment not found";
    }
}
