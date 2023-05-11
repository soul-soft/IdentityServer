using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Services
{
    internal class SessionManager : ISessionManager
    {
        private readonly HttpContext _context;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor.HttpContext
                ?? throw new ArgumentNullException();
        }

        public async Task<AuthenticateResult> AuthenticateAsync(string? scheme)
        {
            return await _context.AuthenticateAsync(scheme);
        }

        public async Task SignInAsync(string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties = null)
        {
            await _context.SignInAsync(scheme, principal, properties);
        }

        public async Task SignOutAsync(string? scheme, AuthenticationProperties? properties = null)
        {
            await _context.SignOutAsync(scheme, properties);
        }
    }
}
