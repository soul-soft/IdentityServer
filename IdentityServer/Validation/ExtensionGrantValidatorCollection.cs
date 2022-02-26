using System.Collections;

namespace IdentityServer.Validation
{
    internal class ExtensionGrantValidatorCollection : IEnumerable<IExtensionGrantValidator>
    {
        private readonly IEnumerable<IExtensionGrantValidator> _extensions;

        public ExtensionGrantValidatorCollection(IEnumerable<IExtensionGrantValidator> extensions)
        {
            _extensions = extensions;
        }

        public IEnumerator<IExtensionGrantValidator> GetEnumerator()
        {
            return _extensions.GetEnumerator();
        }
       
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_extensions).GetEnumerator();
        }

        public IEnumerable<string> GetExtensionGrantTypes()
        {
            foreach (var item in _extensions)
            {
                yield return item.GrantType;
            }
        }

        public Task<GrantValidationResult> ValidateAsync(ExtensionGrantValidationContext context)
        {
            var validator = GetValidator(context.Request.GrantType);
            if (validator == null)
            {
                throw new InvalidRequestException(string.Format("The identityserver does not support '{0}' authentication", context.Request.GrantType));
            }
            return validator.ValidateAsync(context);
        }

        private IExtensionGrantValidator? GetValidator(string grantType)
        {
            foreach (var item in _extensions)
            {
                if (item.GrantType == grantType)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
