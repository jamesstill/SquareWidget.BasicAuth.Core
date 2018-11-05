using System.Threading.Tasks;

namespace SquareWidget.BasicAuth.Core
{
    public interface IBasicAuthenticationService
    {
        Task<bool> IsValidUserAsync(BasicAuthenticationOptions options, string username, string password);
    }
}
