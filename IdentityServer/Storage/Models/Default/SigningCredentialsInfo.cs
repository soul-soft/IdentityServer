using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Models
{
    public class SigningCredentialsInfo
    {
        public SecurityKey Key { get; }

        public SigningCredentials SigningCredentials { get; }
      
        public string SigningAlgorithm { get; }

        public SigningCredentialsInfo(SigningCredentials signingCredentials, string signingAlgorithm)
        {
            Key = signingCredentials.Key;
            SigningCredentials = signingCredentials;
            SigningAlgorithm = signingAlgorithm;
        }
    }
}
