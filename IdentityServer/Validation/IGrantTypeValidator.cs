namespace IdentityServer.Validation
{
    public interface IGrantTypeValidator
    {
        Task ValidateAsync(string requestedGrantType, IEnumerable<string> allowedGrantTypes);
    }
}
