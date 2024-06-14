using headless_forms_authentication_demo.Authentication;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Common.OpenApi;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace headless_forms_authentication_demo.Composers;

public class AuthenticationComposer : IComposer
{

    public void Compose(IUmbracoBuilder builder)
    {
        builder.AddAuth0MemberAuthentication()
            .AddAuth0MemberJWTBearerAuthentication();

        string clientId = builder.Config["Auth0:ClientId"] ?? "";
        string audience = builder.Config["Auth0:Audience"] ?? "";
        string authorizeUrl = $"{builder.Config["Auth0:Authority"]}/authorize";
        string tokenUrl = $"{builder.Config["Auth0:Authority"]}/oauth/token";
        
        builder.Services.Configure<UmbracoPipelineOptions>(options =>
        {
            options.PipelineFilters.RemoveAll(filter => filter is SwaggerRouteTemplatePipelineFilter);
            options.AddFilter(new SwaggerPipelineFilter(clientId, audience,authorizeUrl,tokenUrl));
        });
           
    }
}