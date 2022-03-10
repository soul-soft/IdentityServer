﻿using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityServer.Validation
{
    internal class TokenValidator : ITokenValidator
    {
        private readonly ISystemClock _systemClock;
        private readonly IdentityServerOptions _options;
        private readonly ISigningCredentialStore _credentials;
        private readonly ITokenStore _referenceTokenStore;

        public TokenValidator(
            ISystemClock systemClock,
            IdentityServerOptions options,
            ISigningCredentialStore credentials,
            ITokenStore referenceTokenStore)
        {
            _options = options;
            _systemClock = systemClock;
            _credentials = credentials;
            _referenceTokenStore = referenceTokenStore;
        }

        public async Task<ClaimsPrincipal> ValidateAsync(string? token)
        {
            IEnumerable<Claim> claims;
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidTokenException("Access token is missing");
            }
            if (token.Length > _options.InputLengthRestrictions.AccessToken)
            {
                throw new InvalidTokenException("Access token too long");
            }
            if (token.Contains('.'))
            {
                claims = await ValidateJwtTokenAsync(token);
            }
            else
            {
                claims = await ValidateReferenceTokenAsync(token);
            }
            var identity = new ClaimsIdentity(_options.AuthenticationOptions.Scheme);
            identity.AddClaims(claims);
            return new ClaimsPrincipal(identity);
        }

        private async Task<IEnumerable<Claim>> ValidateJwtTokenAsync(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                handler.InboundClaimTypeMap.Clear();
                var securityKeys = await _credentials.GetSecurityKeysAsync();                
                var parameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer = _options.Issuer,
                    IssuerSigningKeys = securityKeys,
                    ValidAudience = _options.AuthenticationOptions.ValidAudience,
                    ValidateIssuer = _options.AuthenticationOptions.ValidateIssuer,
                    ValidateAudience = _options.AuthenticationOptions.ValidateAudience,
                    ValidateLifetime = _options.AuthenticationOptions.ValidateLifetime,
                };
                var subject = handler.ValidateToken(token, parameters, out var securityToken);
                return subject.Claims;
            }
            catch (Exception ex)
            {
                throw new InvalidTokenException(ex.Message);
            }
        }

        private async Task<IEnumerable<Claim>> ValidateReferenceTokenAsync(string tokenReference)
        {
            var token = await _referenceTokenStore.FindTokenAsync(tokenReference);
            if (token == null)
            {
                throw new InvalidTokenException("Invalid token");
            }
            if (_options.AuthenticationOptions.ValidateLifetime && token.Expiration < _systemClock.UtcNow.UtcDateTime)
            {
                throw new InvalidTokenException("The access token has expired");
            }
            if (_options.AuthenticationOptions.ValidateIssuer && token.Issuer != _options.Issuer)
            {
                throw new InvalidTokenException("Invalid issuer");
            }
            return token.GetJwtPayloadClaims();
        }
    }
}
