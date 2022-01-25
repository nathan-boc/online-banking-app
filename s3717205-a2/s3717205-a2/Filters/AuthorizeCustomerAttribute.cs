using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using MvcBank.Models;

namespace MvcBank.Filters;

public class AuthorizeCustomerAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var customerID = context.HttpContext.Session.GetInt32(nameof(Customer.CustomerID));
        
        // If the session doesn't contain CustomerID data, redirect to home page
        if(customerID == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
