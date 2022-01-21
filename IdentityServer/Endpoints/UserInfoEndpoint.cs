﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using IdentityServer.Hosting;

namespace IdentityServer.Endpoints
{
    internal class UserInfoEndpoint : EndpointBase
    {
        private readonly IClientStore _clients;
        private readonly IResourceStore _resources;
        private readonly IScopeParser _scopeParser;
        private readonly IScopeValidator _scopeValidator;
        private readonly IUserInfoGenerator _generator;
        private readonly IResourceValidator _resourceValidator;

        public UserInfoEndpoint(
            IClientStore clients,
            IResourceStore resources,
            IScopeParser scopeParser,
            IScopeValidator scopeValidator,
            IUserInfoGenerator generator,
            IResourceValidator resourceValidator)
        {
            _clients = clients;
            _resources = resources;
            _generator = generator;
            _scopeParser = scopeParser;
            _scopeValidator = scopeValidator;   
            _resourceValidator = resourceValidator;
        }

        public override async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var authenticateResult = await context.AuthenticateAsync(IdentityServerAuthDefaults.Scheme);
            if (authenticateResult == null || !authenticateResult.Succeeded)
            {
                return Unauthorized(OpenIdConnectTokenErrors.InvalidRequest, "authentication failed");
            }
            var subject = authenticateResult.Principal;
            var sub = subject.GetSubjectId();
            if (string.IsNullOrWhiteSpace(sub))
            {
                return Unauthorized(OpenIdConnectTokenErrors.InvalidToken, "Sub claim is missing");
            }
            var clientId = subject.GetClientId();
            if (string.IsNullOrWhiteSpace(clientId))
            {
                return Unauthorized(OpenIdConnectTokenErrors.InvalidToken, "ClientId claim is missing");
            }
            var scopes = await _scopeParser.ParseAsync(subject);
            var client = await _clients.GetAsync(clientId);
            if (client == null)
            {
                return Unauthorized(OpenIdConnectTokenErrors.InvalidToken, "Invalid client");
            }
            await _scopeValidator.ValidateAsync(client.AllowedScopes,scopes);
            var resources = await _resources.FindByScopeAsync(scopes);
            await _resourceValidator.ValidateAsync(resources, scopes);
            var response = await _generator.ProcessAsync(new UserInfoRequest(subject, client, resources));
            return new UserInfoResult(response);
        }
    }
}
