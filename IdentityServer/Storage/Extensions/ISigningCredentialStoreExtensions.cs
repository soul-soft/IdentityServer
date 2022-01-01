using IdentityServer.Storage;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer
{
    public static class ISigningCredentialStoreExtensions
    {
        public static async Task<SigningCredentials?> GetSigningCredentialsAsync(this ISigningCredentialStore credentialStore, IEnumerable<string> algorithms)
        {
            var credentials = await credentialStore.GetAllSigningCredentialsAsync();
            if (credentials.Any())
            {
                if (algorithms.Count()==0)
                {
                    return credentials.First();
                }

                var credential = credentials.FirstOrDefault(c => algorithms.Contains(c.Algorithm));
               
                if (credential is null)
                {
                    throw new InvalidOperationException($"No signing credential for algorithms ({algorithms.ToSpaceSeparatedString()}) registered.");
                }

                return credential;
            }
            return null;
        }
    }
}
