# Umbraco Forms Headless Member Authentication Demo Site

This is a demo repo to accompany my article about using external authentication providers with Umbraco Forms, available [here](https://mckaskill.com/blog/headless-umbraco-forms-user-authentication)

To use the site, follow these steps:

1. Clone the site

```bash
git clone https://github.com/andrewmckaskill/umbraco-forms-headless-member-authentication
```

2. Sign up for an Auth0 account and create test application (detailed instructions in blog article)
 
3. Configure the user secrets with your domain and clientId

  ```bash
  dotnet user-secrets init
  dotnet user-secrets set Auth0:Domain YOUR_AUTHO_DOMAIN
  dotnet user-secrets set Auth0:ClientId YOUR_AUTH0_CLIENTID
  dotnet user-secrets set Auth0:Authority "https://YOUR_AUTH0_DOMAIN"
  dotnet user-secrets set Auth0:Audience umbraco-api
  ```

4. Run the site with `dotnet run`
