
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core;
using Umbraco.Cms.Web.Common.Security;

namespace headless_forms_authentication_demo.Authentication;

public static class Auth0AuthenticationExtensions
{
    public const string Scheme = "Auth0";

    public static IUmbracoBuilder AddAuth0MemberAuthentication(this IUmbracoBuilder builder)
    {
        builder.Services.ConfigureOptions<Auth0MemberExternalLoginProviderOptions>();

        builder.AddMemberExternalLogins(logins =>
        {
            logins.AddMemberLogin(
                membersAuthenticationBuilder =>
                {
                    membersAuthenticationBuilder.AddAuth0WebAppAuthentication(
                        membersAuthenticationBuilder.SchemeForMembers(Scheme),
                        options =>
                        {
                            // add your client ID and secret here
                            options.Domain = builder.Config["Auth0:Domain"];
                            options.ClientId = builder.Config["Auth0:ClientId"];
                            options.Scope = "openid profile email";
                            options.SkipCookieMiddleware = true;
                            
                        });
                });
        });

        return builder;
    }
    
    private class Auth0MemberExternalLoginProviderOptions : IConfigureNamedOptions<MemberExternalLoginProviderOptions>
    {
        public void Configure(string? name, MemberExternalLoginProviderOptions options)
        {
            if (name is not $"{Constants.Security.MemberExternalAuthenticationTypePrefix}{Scheme}")
            {
                return;
            }

            Configure(options);
        }

        public void Configure(MemberExternalLoginProviderOptions options)
            => options.AutoLinkOptions = new MemberExternalSignInAutoLinkOptions(
                autoLinkExternalAccount: true,
                defaultCulture: null,
                defaultIsApproved: true,
                defaultMemberTypeAlias: Constants.Security.DefaultMemberTypeAlias);
    }
}

