using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;

namespace IdentityServer.Extensions
{
    internal static class FormCollectionExtensions
    {
        public static NameValueCollection AsNameValueCollection(this IFormCollection form)
        {
            var nv = new NameValueCollection();
            foreach (var field in form)
            {
                nv.Add(field.Key, field.Value.First());
            }
            return nv;
        }

        public static NameValueCollection AsNameValueCollection(this IQueryCollection form)
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
