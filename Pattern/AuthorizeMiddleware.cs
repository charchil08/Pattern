using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
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
            string path = context.Request.Path.Value ?? "/Account";

            ITempDataDictionary tempData = _tempDataFactory.GetTempData(context);
            string token = context.Request.Cookies["auth"] ?? "";


            if (IsBypassPath(path))
            {
                if(!string.IsNullOrWhiteSpace(token))
                {
                    string previousUrl = context.Request.Headers["Referer"].ToString();
                    if(string.IsNullOrWhiteSpace(previousUrl))
                    {
                        previousUrl = "/Home";
                    }
                    context.Response.Redirect(previousUrl);
                    return;
                }

                await next(context);
                return;
            }

            
            if (string.IsNullOrWhiteSpace(token))
            {
                bool isXmlHttpRequest = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if(isXmlHttpRequest)
                {
                    RedirectUnAuthorizedOnAjaxCall(context);
                    return;
                }
                
                RedirectToLogin(context, tempData);
                return;
            }

            string secretKey = _configuration.GetValue<string>("JwtSetting:SecretKey");
            JwtHelper jwtHelper = new JwtHelper();
            ClaimsPrincipal? claimsPrincipal = jwtHelper.ValidateJwtToken(token, secretKey);


            if (IsTokenInvalid(claimsPrincipal))
            {
                RedirectToLogin(context, tempData);
                return;
            }

            context.User = claimsPrincipal!;
            

            if (path.StartsWith("/Skill"))
            {
                bool isRoleAdmin = context.User.IsInRole("admin");
                bool isActorAdmin = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Actor)!.Value == "admin";

                if (!isRoleAdmin || !isActorAdmin)
                {
                    RedirectToUnauthorized(context, tempData);
                    return;
                }
            }

            await next(context);
        }


        private bool IsBypassPath(string path)
        {
            string[] bypassPaths = { "/Account", "/Account/Index"};
            return bypassPaths.Contains(path);
        }

        private void RedirectUnAuthorizedOnAjaxCall(HttpContext context)
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(JsonConvert.SerializeObject(new { expiredToken = true }));
            context.Response.Redirect($"/Account");

        }

        private void RedirectToLogin(HttpContext context, ITempDataDictionary tempData)
        {
            string returnUrl = context.Request.Path + context.Request.QueryString;
            tempData["error"] = "Login first !";

            context.Response.Redirect($"/Account?returnUrl={returnUrl}");
        }

        private bool IsTokenInvalid(ClaimsPrincipal? claimsPrincipal)
        {
            return claimsPrincipal == null || !claimsPrincipal.Identity!.IsAuthenticated;
        }

        private void RedirectToUnauthorized(HttpContext context, ITempDataDictionary tempData)
        {
            string previousUrl = context.Request.Headers["Referer"].ToString();
            tempData["error"] = "You are not authorized to open this page!";
            context.Response.Headers.Add("Unauthorized", "true");
            context.Response.Redirect(previousUrl);
        }
    }
}
