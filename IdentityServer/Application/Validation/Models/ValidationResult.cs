namespace IdentityServer.Application
{
    public class ValidationResult
    {
        public bool IsError { get; private set; }

        public string? Description { get; private set; }

        private ValidationResult(bool isError)
        {
            IsError = isError;
        }

        private ValidationResult(string? description, bool isError = true)
        {
            IsError = isError;
            Description = description;
        }

        public static ValidationResult Success()
        {
            return new ValidationResult(true);
        }

        public static Task<ValidationResult> SuccessAsync()
        {
            var result = new ValidationResult(true);
            return Task.FromResult(result);
        }

        public static ValidationResult Error(string? format, params object[] args)
        {
            return new ValidationResult(string.Format(format ?? string.Empty, args));
        }

        public static Task<ValidationResult> ErrorAsync(string format, params object[] args)
        {
            var result = new ValidationResult(string.Format(format, args));
            return Task.FromResult(result);
        }
    }
}
