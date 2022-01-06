using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Application
{
    public enum ProfileDataRequestCaller
    {
        UserInfoEndpoint,
        ClaimsProviderIdentityToken,
        ClaimsProviderAccessToken
    }
}
