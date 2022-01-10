//using Microsoft.Extensions.Primitives;

//namespace Hosting
//{
//    public static class IdentityServerEndpointRouteBuilderExtensions
//    {
//        public static void MapIdentityServerEndpoint(this IEndpointRouteBuilder endpoints)
//        {
//            var dataSource = endpoints.DataSources.OfType<IdentityServerEndpointDataSource>().FirstOrDefault();
//            if (dataSource == null)
//            {
//                dataSource = new IdentityServerEndpointDataSource();
//                endpoints.DataSources.Add(dataSource);
//            }
//        }
//    }

//    public class IdentityServerEndpointDataSource : EndpointDataSource
//    {
//        protected readonly object Lock = new object();
//        private IChangeToken? _changeToken;
//        private List<Endpoint>? _endpoints;
//        private CancellationTokenSource? _cancellationTokenSource;
//        public IdentityServerEndpointDataSource()
//        {

//        }
      
//        public override IReadOnlyList<Endpoint> Endpoints
//        {
//            get
//            {
//                Initialize();
//                return _endpoints ?? new List<Endpoint>();
//            }
//        }

//        public override IChangeToken GetChangeToken()
//        {
//            Initialize();
//            return _changeToken;
//        }

//        private void Initialize()
//        {
//            if (_endpoints == null)
//            {
//                lock (Lock)
//                {
//                    if (_endpoints == null)
//                    {
//                        UpdateEndpoints();
//                    }
//                }
//            }
//        }

//        private void UpdateEndpoints()
//        {
//            lock (Lock)
//            {
//                var endpoints = CreateEndpoints();

//                // See comments in DefaultActionDescriptorCollectionProvider. These steps are done
//                // in a specific order to ensure callers always see a consistent state.

//                // Step 1 - capture old token
//                var oldCancellationTokenSource = _cancellationTokenSource;

//                // Step 2 - update endpoints
//                _endpoints = endpoints;

//                // Step 3 - create new change token
//                _cancellationTokenSource = new CancellationTokenSource();
//                _changeToken = new CancellationChangeToken(_cancellationTokenSource.Token);
//                // Step 4 - trigger old token
//                oldCancellationTokenSource?.Cancel();
//            }
//        }

//        //private List<Endpoint> CreateEndpoints()
//        //{
//        //    return new List<Endpoint>()
//        //    {
//        //        new Endpoint()
//        //    };
//        //}
//    }

//}
