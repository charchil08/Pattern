using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Pattern.Utility;
using System.Security.Claims;

namespace Pattern
{
    public class AuthorizeMiddleware : IMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public AuthorizeMiddleware(IConfiguration configuration, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _configuration = configuration;
            _tempDataFactory = tempDataDictionaryFactory;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string path = context.Request.Path.Value ?? "/Account/";


            if (path == "/" || path == "/Account/" || path == "/Account/Index" || path == "/Home/Index" || path == "/Home/Privacy")
            {
                // Bypass authentication for specific paths
                await next(context);
                return;
            }

            // Extract the token from the request, such as from a header or query string
            string token = context.Request.Cookies["auth"] ?? "";

            if (string.IsNullOrWhiteSpace(token))
            {
                // Redirect to login if no token is provided
                string returnUrl = context.Request.Path + context.Request.QueryString;
                context.Response.Redirect($"/Account/?returnUrl={returnUrl}");
                return;
            }


            // Validate the token and get the claims
            string secrectKey = _configuration.GetValue<string>("JwtSetting:SecretKey");
            JwtHelper jwtHelper = new JwtHelper();
            ClaimsPrincipal? claimsPrincipal = jwtHelper.ValidateJwtToken(token, secrectKey);

            if (claimsPrincipal == null || !claimsPrincipal.Identity!.IsAuthenticated)
            {
                // Redirect to login if token validation fails
                string returnUrl = context.Request.Path + context.Request.QueryString;
                context.Response.Redirect($"/Account/Index?returnUrl={returnUrl}");
                context.Response.Headers.Add("Unauthorized", "true");
                return;
            }


            // Set the user claims in the context
            context.User = claimsPrincipal;


            if (path.StartsWith("/Skill"))
            {
                bool isRoleAdmin = context.User.IsInRole("admin");
                bool isActorAdmin = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Actor).Value == "admin";

                if (!isRoleAdmin || !isActorAdmin)
                {
                    //ITempDataDictionary tempData = _tempDataFactory.GetTempData(context);
                    string previousUrl = context.Request.Headers["Referer"].ToString();
                    //tempData["error"] = "You are not authorized to open this page!";
                    context.Response.Redirect(previousUrl);
                    context.Response.Headers.Add("Unauthorized", "true");
                    return;
                }
            }

            await next(context);
        }
    }
}