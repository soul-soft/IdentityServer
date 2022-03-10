namespace IdentityServer.Models
{
    public class OpenIdConnectErrors
    {
        public const string InvalidRequest = "invalid_request";
        public const string InvalidClient = "invalid_client";
        public const string InvalidGrant = "invalid_grant";
        public const string UnauthorizedClient = "unauthorized_client";
        public const string UnsupportedResponseType = "unsupported_response_type";
        public const string InvalidScope = "invalid_scope";
        public const string AuthorizationPending = "authorization_pending";
        public const string AccessDenied = "access_denied";
        public const string SlowDown = "slow_down";
        public const string ExpiredToken = "expired_token";
        public const string InvalidTarget = "invalid_target";
        public const string InvalidToken = "invalid_token";
        public const string InsufficientScope = "insufficient_scope";
    }
   
    public class OpenIdConnectTokenErrors
    {
        public const string InvalidRequest = "invalid_request";

        public const string InvalidClient = "invalid_client";

        public const string InvalidGrant = "invalid_grant";

        public const string UnauthorizedClient = "unauthorized_client";

        public const string UnsupportedGrantType = "unsupported_grant_type";

        public const string UnsupportedResponseType = "unsupported_response_type";

        public const string InvalidScope = "invalid_scope";
    }
}
