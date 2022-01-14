using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class GrantValidationResult : ValidationResult
    {
        public ClaimsPrincipal Subject { get; } = new ClaimsPrincipal();

        public GrantValidationResult(ClaimsPrincipal subject)
            : base(false)
        {
            Subject = subject;
        }

        protected GrantValidationResult(bool isError, string description = "success")
            : base(isError, description)
        {

        }

        public static new GrantValidationResult Success()
        {
            return new GrantValidationResult(false);
        }

        public static new Task<GrantValidationResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static new GrantValidationResult Error(string description, params object[] args)
        {
            return new GrantValidationResult(true, string.Format(description, args));
        }

        public static new Task<GrantValidationResult> ErrorAsync(string description, params object[] args)
        {
            return Task.FromResult(Error(description, args));
        }

        public static GrantValidationResult Success(string subject, IEnumerable<Claim>? cliams = null)
        {
            var list = new List<Claim>();
            list.Add(new Claim(JwtClaimTypes.Subject, subject));
            if (cliams != null)
            {
                list.AddRange(cliams);
            }
            return new GrantValidationResult(new ClaimsPrincipal(new ClaimsIdentity(list)));
        }

        public static Task<GrantValidationResult> SuccessAsync(string subject, IEnumerable<Claim>? cliams = null)
        {
            return Task.FromResult(Success(subject, cliams));
        }

    }
}
