﻿using IdentityServer.Models;

namespace IdentityServer.EntityFramework.Entities
{
    public class ApiScopeEntity : ResourceEntity
    {
        public bool Required { get; set; } = false;

        public string Scope => Name;

        public static implicit operator ApiScope?(ApiScopeEntity? entity)
        {
            if (entity == null) return null;
            return new ApiScope(entity.Name)
            {
                Required = entity.Required,
                Enabled = entity.Enabled,
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                ShowInDiscoveryDocument = entity.ShowInDiscoveryDocument,
                ClaimTypes = entity.ClaimTypes.Select(s => s.Value).ToArray(),
            };
        }

        public static implicit operator ApiScopeEntity(ApiScope entity)
        {
            return new ApiScopeEntity
            {
                Name = entity.Name,
                Required = entity.Required,
                Enabled = entity.Enabled,
                DisplayName = entity.DisplayName,
                Description = entity.Description,
                ShowInDiscoveryDocument = entity.ShowInDiscoveryDocument,
                ClaimTypes = entity.ClaimTypes.Select(s => new StringEntity(s)).ToArray(),
            };
        }
    }
}
