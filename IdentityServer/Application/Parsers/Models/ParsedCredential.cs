namespace IdentityServer.Application
{
    public class ParsedCredential
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// 凭据
        /// </summary>
        public object? Secret { get; }
        /// <summary>
        /// 凭据类型：IdentityServerConstants.ParsedSecretTypes
        /// </summary>
        public string Type { get; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public ParsedCredential(string id, string type)
        {
            Id = id;
            Type = type;
        }

        public ParsedCredential(string clientId, object credential, string type)
        {
            Id = clientId;
            Secret = credential;
            Type = type;
        }
    }
}
