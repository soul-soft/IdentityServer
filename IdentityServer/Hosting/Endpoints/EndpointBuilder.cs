namespace IdentityServer.Hosting
{
    public class EndpointBuilder
    {
        private readonly IList<object> _metadata = new List<object>();

        public void Add(object metadata)
        {
            _metadata.Add(metadata);
        }

        internal IList<object> Build()
        {
            return _metadata;
        }
    }
}
