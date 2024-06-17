using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Umbraco.Cms.Api.Common.OpenApi;

namespace headless_forms_authentication_demo.Composers;

public class SwaggerPipelineFilter : SwaggerRouteTemplatePipelineFilter
{
    private const string ApiName = "API";
    private readonly string _clientId;
    private readonly string _audience;
    private readonly string _authorizeUrl;
    private readonly string _tokenUrl;
    
    public SwaggerPipelineFilter(
        string clientId,
        string audience,
        string authorizeUrl,
        string tokenUrl) : base(ApiName)
    {
        _clientId = clientId;
        _audience = audience;
        _authorizeUrl = authorizeUrl;
        _tokenUrl = tokenUrl;
    }

    protected override void SwaggerUiConfiguration(SwaggerUIOptions swaggerUiOptions, SwaggerGenOptions swaggerGenOptions,
        IApplicationBuilder applicationBuilder)
    {
        base.SwaggerUiConfiguration(swaggerUiOptions, swaggerGenOptions, applicationBuilder);
        
        swaggerUiOptions.OAuthClientId(_clientId);
        swaggerUiOptions.OAuthAdditionalQueryStringParams(new Dictionary<string, string>()
        {
            {"audience", _audience}
        });
        swaggerGenOptions.DocumentFilter<Authentication.SwaggerOAuthAuthorizationDocumentFilter>(
            "default",
            _authorizeUrl,
            _tokenUrl
        );
    }
} 