using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace EquipHostAPI.Infrastructure;
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private const string ApiKeyHeaderName = "X-API-KEY";

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var apiKey = _configuration.GetSection("ApiSettings:ApiKey").Value;

        if (!httpContext.Request.Headers.ContainsKey(ApiKeyHeaderName) ||
            httpContext.Request.Headers[ApiKeyHeaderName] != apiKey)
        {
            httpContext.Response.StatusCode = 401; 
            await httpContext.Response.WriteAsync("Invalid or missing API key.");
            return;
        }

        await _next(httpContext);
    }
}

