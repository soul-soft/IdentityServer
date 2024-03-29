﻿namespace IdentityServer.Models
{
    public class ValidationErrors
    {
        public const string InvalidNone = "invalid_none";
        public const string InvalidGrant = "invalid_grant";
        public const string InvalidPostLogoutRedirectUri = "invalid_post_logout_redirect_uri";
        public const string InvalidCodeVerifier = "invalid_code_verifier";
        public const string InvalidClient = "invalid_client";
        public const string InvalidRequest = "invalid_request";
        public const string UnauthorizedClient = "unauthorized_client";
        public const string UnsupportedResponseType = "unsupported_response_type";
        public const string InvalidScope = "invalid_scope";
        public const string InvalidState = "invalid_state";
        public const string AuthorizationPending = "authorization_pending";
        public const string AccessDenied = "access_denied";
        public const string UnsupportedGrantType = "unsupported_grant_type";
        public const string SlowDown = "slow_down";
        public const string ExpiredToken = "expired_token";
        public const string InvalidTarget = "invalid_target";
        public const string InvalidToken = "invalid_token";
        public const string InsufficientScope = "insufficient_scope";
    }
}
