using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors.ResetPassword
{
    public record struct ResetPasswordRequestExpiredError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;

        public string Message => "ResetPasswordRequest has Been Expired, please make another request";
    }
}
