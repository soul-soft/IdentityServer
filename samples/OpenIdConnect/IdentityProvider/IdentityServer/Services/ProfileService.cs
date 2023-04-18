using IdentityServer.Models;
using IdentityServer.Services;
using System.Security.Claims;

namespace IdentityProvider.IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        public Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ProfileClaimsRequest request)
        {
            return Task.FromResult<IEnumerable<Claim>>(Array.Empty<Claim>());
        }

        public Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ProfileClaimsRequest request)
        {
            return Task.FromResult<IEnumerable<Claim>>(Array.Empty<Claim>());
        }

        public Task<IEnumerable<Claim>> GetProfileClaimsAsync(ProfileClaimsRequest request)
        {
            var claims = new List<Claim>();
            if (request.ClaimTypes.Any(a => a == JwtClaimTypes.NickName))
            {
                claims.Add(new Claim(JwtClaimTypes.NickName,"admin"));
            }
            if (request.ClaimTypes.Any(a => a == JwtClaimTypes.Gender))
            {
                claims.Add(new Claim(JwtClaimTypes.Gender, "boy"));
            }
            if (request.ClaimTypes.Any(a => a == JwtClaimTypes.Email))
            {
                claims.Add(new Claim(JwtClaimTypes.Gender, "1448376744@qq.com"));
            }
            return Task.FromResult<IEnumerable<Claim>>(claims);
        }

        public Task<bool> IsActiveAsync(IsActiveRequest request)
        {
            return Task.FromResult(true);
        }
    }
}
