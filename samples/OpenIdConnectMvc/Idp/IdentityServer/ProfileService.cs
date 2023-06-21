using System.Security.Claims;
using IdentityServer.Extensions;
using IdentityServer.Models;
using IdentityServer.Services;

namespace Idp.IdentityServer
{
    public class ProfileService : IProfileService
    {
        readonly IHttpContextAccessor _accessor;

        public ProfileService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ProfileClaimsRequest request)
        {
            var claims = new List<Claim>();
            var clientId = request.Subject.GetClientId();
            var subjectId = request.Subject.GetSubjectId();
            return Task.FromResult<IEnumerable<Claim>>(claims);
        }

        public Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ProfileClaimsRequest request)
        {
            var claims = new List<Claim>();
            var clientId = request.Subject.GetClientId();
            var subjectId = request.Subject.GetSubjectId();
            return Task.FromResult<IEnumerable<Claim>>(claims);
        }


        public Task<IEnumerable<Claim>> GetUserInfoClaimsAsync(ProfileClaimsRequest request)
        {
            var claims = new List<Claim>();
            if (request.ClaimTypes.Any(a => a == JwtClaimTypes.NickName))
            {
                claims.Add(new Claim(JwtClaimTypes.NickName, "tom"));
            }
            if (request.ClaimTypes.Any(a => a == JwtClaimTypes.Picture))
            {
                claims.Add(new Claim(JwtClaimTypes.NickName, "http://fff.png"));
            }
            return Task.FromResult<IEnumerable<Claim>>(claims);
        }

        public Task<bool> IsActiveAsync(IsActiveRequest request)
        {
            return Task.FromResult(true);
        }
    }
}
