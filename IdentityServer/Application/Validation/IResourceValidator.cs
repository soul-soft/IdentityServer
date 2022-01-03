namespace IdentityServer.Application
{
    public interface IResourceValidator
    {
        Task<ResourceValidationResult> ValidateAsync(ResourceValidationRequest request);
    }
}
