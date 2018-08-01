using System.Security.Claims;
using System.Threading.Tasks;

namespace PG.Api.Auth
{
    public interface IJwtAuthFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}
