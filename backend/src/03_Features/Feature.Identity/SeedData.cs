using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;

namespace Simplify.Feature.Identity
{
    public static class SeedData
    {
        public static IEnumerable<User> GetUsers()
        {
            yield return new User
            {
                Id = new Guid(),
                Email = "demo@example.com",
                Roles = new HashSet<string> { "SeedData" },
                NormalizedEmail = "demo@example.com".ToUpper(CultureInfo.InvariantCulture),
                AccessFailedCount = 0,
                EmailConfirmed = false,
                UserName = "1@test.com",
                Claims = new List<Claim> { new Claim(ClaimTypes.Name, "displayName", null, SimplifyIdentityExtensions.Constants.IdentityIssuer) },
                NormalizedUserName = "demo@example.com".ToUpper(CultureInfo.InvariantCulture),
                PhoneNumber = "00449223344669",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false
            };
        }
    }
}