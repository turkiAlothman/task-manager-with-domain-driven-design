using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors.Invites
{
    public record struct AlreadyInvitedError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string Message => "this email already invited";
    }
}
