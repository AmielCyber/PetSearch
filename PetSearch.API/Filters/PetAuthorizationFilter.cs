using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PetSearchAPI.Filters;

public class PetAuthorizationFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var values = context.HttpContext.Request.Headers["Authorization"];
        if (string.IsNullOrWhiteSpace(values.ToString()))
        {
            context.Result = new UnauthorizedResult();
        }
    }
}