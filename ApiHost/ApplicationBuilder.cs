//namespace ApiHost
//{
//    public delegate Task RequestDelegate(CqrsContext context);
//    public class MyApplicationBuilder
//    {
//        private readonly List<Func<RequestDelegate, RequestDelegate>> _item = new List<Func<RequestDelegate, RequestDelegate>>();

//        public void UseMiddleware(Func<CqrsContext, Func<Task>, Task> middleware)
//        {
//            _item.Add(next =>
//            {
//                return async context =>
//                {
//                    await middleware(context, async () =>
//                    {
//                        await next(context);
//                    });
//                };
//            });
//        }

//        public RequestDelegate Build()
//        {
//            RequestDelegate app = (next) =>
//            {
//                Console.WriteLine("final中间件：最终处理逻辑");
//                return Task.CompletedTask;
//            };
//            foreach (var item in _item)
//            {
//                app = item(app);
//            }
//            return app;
//        }
//    }
//    public class CqrsContext
//    {
//        public object Request { get; }
//        public object Response { get; }
//    }
//}
