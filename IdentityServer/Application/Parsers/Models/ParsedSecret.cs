namespace IdentityServer.Application
{
    public class ParsedSecret
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// 凭据
        /// </summary>
        public object? Credential { get; set; }
        /// <summary>
        /// 凭据类型：IdentityServerConstants.ParsedSecretTypes
        /// </summary>
        public string Type { get; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();
        public ParsedSecret(string id, string type)
        {
            Id = id;
            Type = type;
        }
        public ParsedSecret(string clientId, object credential, string type)
        {
            Id = clientId;
            Credential = credential;
            Type = type;
        }
    }
}
