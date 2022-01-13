using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class GrantValidationResult
    {
        public bool IsError { get; }

        public string Description { get; }

        public string? SubjectId { get; }

        public IReadOnlyCollection<Claim> Claims { get; } = new List<Claim>();

        protected GrantValidationResult(bool isError, string description = "success")
        {
            IsError = isError;
            Description = description;
        }

        protected GrantValidationResult(string? subject, IEnumerable<Claim> claims)
           : this(false)
        {
            SubjectId = subject;
            Claims = claims.ToList();
        }



        public static GrantValidationResult Error(string description, params object[] args)
        {
            return new GrantValidationResult(true, string.Format(description, args));
        }

        public static Task<GrantValidationResult> ErrorAsync(string description, params object[] args)
        {
            return Task.FromResult(Error(description, args));
        }

        public static GrantValidationResult Success()
        {
            return new GrantValidationResult(null, new List<Claim>());
        }

        public static GrantValidationResult Success(string subject)
        {
            return new GrantValidationResult(subject, new List<Claim>());
        }

        public static GrantValidationResult Success(string subject, IEnumerable<Claim> claims)
        {
            return new GrantValidationResult(subject, claims);
        }

        public static GrantValidationResult Success(IEnumerable<Claim> claims)
        {
            return new GrantValidationResult(null, claims);
        }
        
        public static Task<GrantValidationResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<GrantValidationResult> SuccessAsync(string subject)
        {
            return Task.FromResult(Success(subject));
        }

        public static Task<GrantValidationResult> SuccessAsync(string subject, IEnumerable<Claim> claims)
        {
            return Task.FromResult(Success(subject, claims));
        }

        public static Task<GrantValidationResult> SuccessAsync(IEnumerable<Claim> claims)
        {
            return Task.FromResult(Success(claims));
        }
    }
}
