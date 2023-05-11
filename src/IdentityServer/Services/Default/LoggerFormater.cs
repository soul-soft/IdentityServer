using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Services.Default
{
    internal class LoggerFormater : ILoggerFormater
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggerFormater(
            ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = loggerFactory.CreateLogger("IdentityServer.Hosting");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogRequestAsync()
        {
            if (!_logger.IsEnabled(LogLevel.Debug))
            {
                return;
            }
            if (_httpContextAccessor.HttpContext == null)
            {
                return;
            }
            var context = _httpContextAccessor.HttpContext;

            NameValueCollection parameters;
            if (HttpMethods.IsGet(context.Request.Method))
            {
                parameters = context.Request.Query.AsNameValueCollection();
            }
            else
            {
                parameters = (await context.Request.ReadFormAsync()).AsNameValueCollection();
            }
            var map = new Dictionary<string, object?>();
            if (parameters.HasKeys())
            {
                foreach (var key in parameters.AllKeys)
                {
                    var values = parameters.GetValues(key);
                    var value = values?.Length > 1 ? values[0] : values?.FirstOrDefault();
                    map.Add(key!, value);
                }
            }
            var body = JsonSerializer.Serialize(map, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
            var sb = new StringBuilder();
            sb.AppendFormat("Url：{0}\r\n", context.Request.Path);
            sb.AppendFormat("Method：{0}\r\n", context.Request.Method);
            sb.AppendFormat("Body：{0}\r\n", body);
            _logger.LogDebug(sb.ToString());
        }
    }
}
