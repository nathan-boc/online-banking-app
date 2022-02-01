using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MvcBank.Filters;

public class AuthorizeAdminAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var adminKey = context.HttpContext.Session.GetInt32("Admin");
        
        // If the session doesn't contain CustomerID data, redirect to home page
        if(adminKey == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
