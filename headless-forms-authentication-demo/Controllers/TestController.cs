using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.Controllers;

namespace headless_forms_authentication_demo.Controllers;

public class TestController : UmbracoApiController
{
    
    public IActionResult GetMemberDetails()
    {
        if (User?.Identity?.IsAuthenticated ?? false)
        {
            return Ok($"Logged in as {User.Identity.Name}");
        }
        
        return new UnauthorizedObjectResult("Not authorized");
        
    }
}