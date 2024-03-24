using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors.ResetPassword
{
    public record struct ResetPasswordAlreadyRequestedError : IError
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string Message => "this email already requested to reset password , please check your email";
    }
}
