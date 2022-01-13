namespace IdentityServer.Validation
{
    public class ValidationResult
    {
        public bool IsError { get; }

        public string Description { get; }

        protected ValidationResult(bool isError, string description = "success")
        {
            IsError = isError;
            Description = description;
        }

        public static ValidationResult Success()
        {
            return new ValidationResult(false, "success");
        }

        public static Task<ValidationResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static ValidationResult Error(string description, params object[] args)
        {
            return new ValidationResult(true, string.Format(description, args));
        }

        public static Task<ValidationResult> ErrorAsync(string description, params object[] args)
        {
            return Task.FromResult(Error(description, args));
        }
    }
}
