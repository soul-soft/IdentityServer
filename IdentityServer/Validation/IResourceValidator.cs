﻿namespace IdentityServer.Validation
{
    public interface IResourceValidator
    {
        Task ValidateAsync(IEnumerable<string> scopes, ResourceCollection resources);
    }
}