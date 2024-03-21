using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace TaskManager.HttpExtensions
{
    public static class HttpContextExtensions
    {

        public static string? GetByName(this IEnumerable<Claim> input, string name)
        {
            return input.Where(c => c.Type == name).Select(c => c.Value).FirstOrDefault();
        }
        
        // get the authenticated user id directly from HttpContext(in controllers)
        public static int GetUserID(this IHttpContextAccessor context)
        {
            return int.Parse( context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }


        // get authenicated user if from ClaimsPrincipal (in rezor pages)
        public static int GetUserId(this ClaimsPrincipal input)
        {
            return int.Parse(input.FindFirstValue(ClaimTypes.NameIdentifier));
        } 

        public static async Task<string> ReadAsStringAsync(this Stream Body)
        {
            using StreamReader reader = new (Body,leaveOpen:false);
            string bodyString =  await reader.ReadToEndAsync();
            return bodyString;
        }

        public static bool IsManager(this IHttpContextAccessor context)
        {
            return bool.Parse(context.HttpContext.User.FindFirstValue("manager") ?? "false");
        }
    }
}
