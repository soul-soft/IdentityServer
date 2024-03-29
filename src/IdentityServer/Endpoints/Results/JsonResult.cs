﻿using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace IdentityServer.Endpoints
{
    public class JsonResult : IEndpointResult
    {
        private readonly string _content;

        public JsonResult(string content)
        {
            _content = content;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync(_content, System.Text.Encoding.UTF8);
        }
    }
}
