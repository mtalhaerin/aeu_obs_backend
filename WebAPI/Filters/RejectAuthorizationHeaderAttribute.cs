using Business.Features.CQRS._Generic.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace WebAPI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RejectAuthorizationHeaderAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasAuthHeader = context.HttpContext.Request.Headers.ContainsKey("Authorization");

            if (hasAuthHeader)
            {
                context.Result = new BadRequestObjectResult(
                    new
                    {
                        IsSuccess = false,
                        Message = "Authorization header is not allowed for this endpoint.",
                        StatusCode = 400
                    }
                );
            }
        }
    }
}