using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors.Comments
{
    public record struct UnathorizedCommentDeleteError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
        public string Message => "only the sender of the comment can delete the message";
    }
}
