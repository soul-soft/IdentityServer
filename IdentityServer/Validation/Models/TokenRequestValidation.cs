﻿using System.Collections.Specialized;
using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class TokenRequestValidation
    {
        public Client Client { get; }
        public string GrantType { get; set; }
        public Resources Resources { get; }
        public NameValueCollection Body { get; }
        public IdentityServerOptions Options { get; }

        public TokenRequestValidation(
            Client client,
            string grantType,
            Resources resources,
            NameValueCollection body,
            IdentityServerOptions options)
        {
            Client = client;
            Options = options;
            GrantType = grantType;
            Resources = resources;
            Body = body;
        }
    }
}
