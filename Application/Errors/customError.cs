using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors
{
    public class customError : IError
    {
        public string Message { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set;} = HttpStatusCode.OK;
    }
}
