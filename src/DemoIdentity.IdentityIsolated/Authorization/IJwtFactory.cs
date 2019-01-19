using System.Collections.Generic;
using System.Security.Claims;

namespace DemoIdentity.IdentityIsolated.Authorization
{
    public interface IJwtFactory
    {
        string GenerateEncodedToken(string userName,
            ClaimsIdentity identity,
            Dictionary<string, object> payloads);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}
