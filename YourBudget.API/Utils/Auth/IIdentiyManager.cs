using System.Security.Claims;

namespace YourBudget.API.Utils.Auth
{
    public interface IIdentiyManager
    {
        ClaimsIdentity GetIdentity(string username, string password);
    }
}
