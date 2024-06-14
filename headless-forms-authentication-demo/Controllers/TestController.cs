using headless_forms_authentication_demo.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.Controllers;

namespace headless_forms_authentication_demo.Controllers;

[ApiExplorerSettings(GroupName = "Test")]
public class TestController : UmbracoApiController
{
    [HttpGet, Route("/api/test/getMemberDetails")]
    public IActionResult GetMemberDetails()
    {
        if (User?.Identity?.IsAuthenticated ?? false)
        {
            return Ok($"Logged in as {User.Identity.Name}");
        }
        
        return new UnauthorizedObjectResult("Not authorized");
        
    }
    
    [Authorize(AuthenticationSchemes = Auth0JWTExtensions.Auth0JWTBearerSchemeName)]
    [HttpGet, Route("/api/test/getMemberDetailsJwt")]
    public IActionResult GetMemberDetailsJwt()
    {
        if (User?.Identity?.IsAuthenticated ?? false)
        {
            return Ok($"Logged in as {User.Identity.Name}");
        }
        
        return new UnauthorizedObjectResult("Not authorized");
        
    }
}