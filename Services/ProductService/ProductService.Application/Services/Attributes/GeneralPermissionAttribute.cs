using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Domain.Entities;
using ProductService.InfrastructureContract.Interfaces.Query.Generic;
using System.Security.Claims;

namespace ProductService.Application.Services.Attributes
{
    public class GeneralPermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
                context.Result = new ForbidResult();
                return;

            }
            var user = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (user == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var action = context.HttpContext.Request.RouteValues["action"]?.ToString();
            var controller = context.HttpContext.Request.RouteValues["controller"]?.ToString();
            var userPermissionService = context.HttpContext.RequestServices.GetRequiredService<IGenericQueryRepository<LocalUserPermissionEntity>>();
            var PermissionsService = context.HttpContext.RequestServices.GetRequiredService<IGenericQueryRepository<LocalPermissionEntity>>();
            var hasRequiredPermission = await PermissionsService.GetQueryable()
                .AnyAsync(p => p.Resource == controller && p.Action == action && userPermissionService.GetQueryable().Any(up => up.PermissionId == p.Id && up.UserId == user));

            if (!hasRequiredPermission)
            {
                context.Result = new ForbidResult();
                return;
            }


        }


    }
}

