using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccountService.Application.Services.Attributes
{
    public class ProductServiceAccessAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Api-Key", out var apiKey))
            {
                context.Result = new UnauthorizedResult(); 
                return;
            }

            
            if (string.IsNullOrWhiteSpace(apiKey) || apiKey != "skdfj349werhch88@#@#")
            {
                context.Result = new UnauthorizedResult(); 
                return;
            }
        }
    }
}
