using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Models
{
    public class SigningCredentialsDescriptor
    {
        public SecurityKey Key { get; }

        public SigningCredentials SigningCredentials { get; }
      
        public string SigningAlgorithm { get; }

        public SigningCredentialsDescriptor(SigningCredentials signingCredentials, string signingAlgorithm)
        {
            Key = signingCredentials.Key;
            SigningCredentials = signingCredentials;
            SigningAlgorithm = signingAlgorithm;
        }
    }
}
