using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using OpenIddict.Abstractions;
using Umbraco.Cms.Core;
using Umbraco.Cms.Web.Common.Security;

namespace headless_forms_authentication_demo.Authentication;

public static class Auth0JWTExtensions
{
    public const string Auth0JWTBearerSchemeName = "Auth0JWTBearer";

    public static IUmbracoBuilder AddAuth0MemberJWTBearerAuthentication(this IUmbracoBuilder builder)
    {
        // Add an additional scheme to do 
        builder.Services.AddAuthentication()
            .AddJwtBearer(
                Auth0JWTBearerSchemeName,
                options =>
                {
                    options.Authority = builder.Config["Auth0:Authority"];
                    options.TokenValidationParameters.ValidateAudience = false;

                    options.Events = new JwtBearerEvents()
                    {
                        OnTokenValidated = async (ctx) =>
                        {
                            
                            /* The following claims are needed, in this order:
                             * 
                             * 1. NameIdentifier - ALWAYS required
                             *    Used in the database to link the external user and the umbraco member 
                             *
                             * 2. Email - required for auto-LINK and auto-CREATE
                             *    Used to find an umbraco member if no link exists
                             *
                             * 3. Name - required for auto-CREATE
                             *    Used to create a new umbraco member if no member found and auto-CREATE
                             *    is turned on
                             *
                             * If these claims don't exist or other claims do, you can re-map them here.
                             */
                            var providerUserId = ctx.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                            var email = ctx.Principal?.FindFirstValue(ClaimTypes.Email);
                            var name = ctx.Principal?.FindFirstValue(ClaimTypes.Name);

                            // For Auth0 we remap the "name" claim to ClaimTypes.Name
                            name = ctx.Principal?.FindFirstValue(JwtRegisteredClaimNames.Name);
                            if (!String.IsNullOrEmpty(name))
                            (ctx.Principal.Identity as ClaimsIdentity).AddClaim(ClaimTypes.Name, name);
                            
                            
                            if (String.IsNullOrEmpty(providerUserId))
                            {
                                ctx.Fail("Required claims missing from external provider details");
                                return;
                            }

                            // create the external login info for the member external login scheme
                            var loginInfo = new ExternalLoginInfo(ctx.Principal,
                                Constants.Security.MemberExternalAuthenticationTypePrefix +
                                Auth0AuthenticationExtensions.Scheme,
                                providerUserId,
                                "Member Auth0");

                            // get a reference to umbraco's sign in manager
                            var memberSignInManager =
                                ctx.HttpContext.RequestServices.GetRequiredService<IMemberSignInManager>();
                            
                            // try to sign the user in based on the ExternalLoginInfo we created above
                            // if successful, the new Umbraco principal will be available on the current httpContext
                            var result = await memberSignInManager.ExternalLoginSignInAsync(loginInfo, false, true);
                            if (result != SignInResult.Success)
                            {
                                ctx.Fail("Unable to sign user in");
                                return;
                            }

                            // Set the CONTEXT principal to the umbraco principal created during externalLoginSignIn. 
                            // The HttpContext will be overwritten with the principal from the event's context
                            // once the event is complete.
                            ctx.Principal = ctx.HttpContext.User;
                        },
                    };

                });

        return builder;
    }
}