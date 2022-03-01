using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;

namespace IdentityServer.Extensions
{
    public static class FormCollectionExtensions
    {
        internal static NameValueCollection AsNameValueCollection(this IFormCollection form)
        {
            var nv = new NameValueCollection();
            foreach (var field in form)
            {
                nv.Add(field.Key, field.Value.First());
            }
            return nv;
        }
    }
}
