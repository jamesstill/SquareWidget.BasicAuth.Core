# SquareWidget.BasicAuthentication.Core

Yet another basic authentication handler for .NET Standard 2.1. Supports IdentityServer4 or a database.  

### Status

[![Build Status](https://jamesstill.visualstudio.com/SquareWidget.BasicAuth.Core/_apis/build/status/SquareWidgetBasicAuthCore%20-%20CI)](https://jamesstill.visualstudio.com/SquareWidget.BasicAuth.Core/_build/latest?definitionId=11)

### Register in Startup

```
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddMvcCore()
            .AddAuthorization();

        // If you want to authenticate against IdentityServer4
        var authenticationUrl = "https://identityserver.io";
        var scope = "api1";

        // If you want to authenticate against a database
        var connectionString = "Server=tcp:serverName;Database=db;User ID=uid;Password=pwd";

        services
            .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
            .AddBasicAuthentication<BasicAuthenticationService>(o =>
            {
                o.DiscoveryUrl = authenticationUrl;
                o.Scope = scope;
                o.ConnectionString = connectionString;
            });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        // other settings

        app.UseAuthentication();
        app.UseMvc();
    }
}
```

You have to implement a BasicAuthenticationService in your code. To call a discovery endpoint on IdentityServer4:

```
public class BasicAuthenticationService : IBasicAuthenticationService
{
    public Task<bool> IsValidUserAsync(BasicAuthenticationOptions options, string username, string password)
    {
        var discoveryClient = new DiscoveryClient(options.DiscoveryUrl);
        var discoveryResponse = discoveryClient.GetAsync().Result;
        if (discoveryResponse.IsError)
        {
            return Task.FromResult(false);
        }

        var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, username, password);
        var tokenResponse = tokenClient.RequestClientCredentialsAsync(options.Scope).Result;
        if (tokenResponse.IsError)
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }
}
```

Or maybe you want to initialize a repository class that knows how to call a database:

```
public class BasicAuthenticationService : IBasicAuthenticationService
{
    public Task<bool> IsValidUserAsync(BasicAuthenticationOptions options, string username, string password)
    {
        var repository = new UserRepository(options.ConnectionString);
        return Task.FromResult(repository.IsValidUser(username, password));
    }                       
}
```


## Versioning

Version 2.1 targeting ASP.NET Standard 2.1 

## Authors

[James Still](http://www.squarewidget.com)

## License

This project is licensed under the MIT License.

## Acknowledgments

* [Joonas Westlin](https://joonasw.net/view/creating-auth-scheme-in-aspnet-core-2)