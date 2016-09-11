using System;
using System.Collections.Generic;
using System.Security.Claims;
namespace Users.Infrastructure
{
    public static class LocationClaimsProvider
    {
        public const string ISSUER = "RemoteClaims";
        public static IEnumerable<Claim> GetClaims(ClaimsIdentity user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("AuthorizeTime", DateTime.Now.ToString(), ClaimValueTypes.DateTime, ISSUER));

            return claims;
        }
    }
}