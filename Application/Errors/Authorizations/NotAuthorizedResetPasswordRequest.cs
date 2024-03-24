using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors.Authorizations
{
    public record struct NotAuthorizedResetPasswordRequest : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

        public string Message => "";
    }
}
