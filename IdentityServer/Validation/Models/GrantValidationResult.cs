using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class GrantValidationResult : ValidationResult
    {
        public string? SubjectId { get; }

        public IReadOnlyCollection<Claim> Claims { get; } = new List<Claim>();

        protected GrantValidationResult(bool isError, string description)
            : base(isError, description)
        {

        }

        protected GrantValidationResult(string? subject, IEnumerable<Claim> claims)
           : base(false)
        {
            SubjectId = subject;
            Claims = claims.ToList();
        }

        public static new GrantValidationResult Error(string description, params object[] args)
        {
            return new GrantValidationResult(true, string.Format(description, args));
        }

        public static new Task<GrantValidationResult> ErrorAsync(string description, params object[] args)
        {
            return Task.FromResult(Error(description, args));
        }

        public static GrantValidationResult Result(string? subject, IEnumerable<Claim>? claims = null)
        {
            return new GrantValidationResult(subject, claims ?? new List<Claim>());
        }

        public static GrantValidationResult Result(IEnumerable<Claim> claims)
        {
            return new GrantValidationResult(null, claims);
        }

        public static Task<GrantValidationResult> ResultAsync(string? subject = null, IEnumerable<Claim>? claims = null)
        {
            return Task.FromResult(Result(subject, claims ?? new List<Claim>()));
        }

        public static Task<GrantValidationResult> ResultAsync(IEnumerable<Claim> claims)
        {
            return Task.FromResult(Result(null, claims));
        }
    }
}
