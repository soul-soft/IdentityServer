namespace IdentityServer.Configuration
{
    public class IdentityServerOptions
    {
        /// <summary>
        /// 签发url
        /// </summary>
        public string IssuerUri { get; set; } = string.Empty;
        /// <summary>
        /// 存储key
        /// </summary>
        public string StorageKeyPrefix { get; set; } = "IdentityServer";
        /// <summary>
        /// 是否使用小写的签发url
        /// </summary>
        public bool LowerCaseIssuerUri { get; set; } = true;
        /// <summary>
        /// 是否包含终结点的错误详情
        /// </summary>
        public bool IncludeEndpointErrorDetails { get; set; } = true;
        /// <summary>
        /// 输入长度限制
        /// </summary>
        public InputLengthRestrictions InputLengthRestrictions { get; set; } = new InputLengthRestrictions();
        /// <summary>
        /// 终结点选项
        /// </summary>
        public EndpointsOptions Endpoints { get; set; } = new EndpointsOptions();
        /// <summary>
        /// token中的jwt类型
        /// </summary>
        public string? AccessTokenJwtType { get; set; } = "at+jwt";
        /// <summary>
        /// 是否使用逗号连接jwt中的scopes
        /// </summary>
        public bool EmitScopesAsCommaDelimitedStringInJwt { get; set; } = true;
        /// <summary>
        /// 认证方式
        /// </summary>
        public string AuthenticationMethod { get; set; } = EndpointAuthenticationMethods.PostBody;
    }
}
