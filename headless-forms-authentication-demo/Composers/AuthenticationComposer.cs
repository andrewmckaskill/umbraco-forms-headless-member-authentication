using headless_forms_authentication_demo.Authentication;
using Umbraco.Cms.Core.Composing;

namespace headless_forms_authentication_demo.Composers;

public class AuthenticationComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.AddAuth0MemberAuthentication()
            .AddAuth0MemberJWTBearerAuthentication();

    }
}