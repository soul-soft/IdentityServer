﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Services
{
    public interface ISessionManager
    {
        Task<AuthenticateResult> AuthenticateAsync(string? scheme);
        Task SignInAsync(string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties = null);
        Task SignOutAsync(string? scheme, AuthenticationProperties? properties = null);
    }
}
