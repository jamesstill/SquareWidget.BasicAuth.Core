using Microsoft.AspNetCore.Authentication;

namespace SquareWidget.BasicAuth.Core
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string ConnectionString { get; set; }
        public string DiscoveryUrl { get; set; }
        public string Scope { get; set; }
    }
}