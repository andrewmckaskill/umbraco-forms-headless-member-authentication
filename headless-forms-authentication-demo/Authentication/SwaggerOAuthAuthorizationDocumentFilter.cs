using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace headless_forms_authentication_demo.Authentication;

public class SwaggerOAuthAuthorizationDocumentFilter : IDocumentFilter
{
    private readonly string _documentName;
    private readonly Uri _authorizeUri;
    private readonly Uri _tokenUri;

    public SwaggerOAuthAuthorizationDocumentFilter(
        string documentName,
        string authorizeUrl,
        string tokenUrl)
    {
        _documentName = documentName;
        

        _authorizeUri = new Uri(authorizeUrl);
        _tokenUri= new Uri(tokenUrl);

    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (context.DocumentName != _documentName)
            return;

        var oauthSecurityScheme = new OpenApiSecurityScheme()
        {
            Name = "oauth",
            Type = SecuritySchemeType.OAuth2,

            Flows = new OpenApiOAuthFlows()
            {

                AuthorizationCode = new OpenApiOAuthFlow()
                {
                    AuthorizationUrl = _authorizeUri,
                    TokenUrl = _tokenUri,
                    Scopes = new Dictionary<string, string>
                    {
                        { "openid", "openid" },
                        { "profile", "profile"}
                    }
                }
            }
        };

        swaggerDoc.Components.SecuritySchemes.Add(oauthSecurityScheme.Name, oauthSecurityScheme);
        swaggerDoc.SecurityRequirements.Add(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = oauthSecurityScheme.Name
                    }
                },
                new string[] { "openid" }
            }
        });
    }

}