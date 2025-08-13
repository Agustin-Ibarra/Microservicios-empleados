using System.Collections.Concurrent;
using System.Diagnostics;

namespace MicroserviceEmployee.Api.Logs;

public class Loggin
{
  private readonly RequestDelegate? _next;
  private readonly ILogger<Loggin> _logger;
  private readonly ConcurrentDictionary<string, int> _ipRequestCount = new();
  public Loggin(RequestDelegate next, ILogger<Loggin> logger)
  {
    _logger = logger;
    _next = next;
  }

  public async Task Invoke(HttpContext context)
  {
    var date = DateTime.Now;
    var time = Stopwatch.StartNew();
    if (_next != null && _logger != null)
    {
      await _next(context);
      time.Stop();
      var request = context.Request;
      var response = context.Response;
      var ipString = context.Connection.RemoteIpAddress?.ToString() ?? "unknown IP";
      _ipRequestCount.AddOrUpdate(ipString, 1, (_, count) => count + 1);
      if (response.StatusCode < 400)
      {
        _logger.LogInformation("Date: {date}, Method: {Method} {Path}, StatusCode: {StatusCode}, in {ElapsedMilliseconds}ms IP address {IP} countRequest {count}",
        date,
        request.Method,
        request.Path,
        response.StatusCode,
        time.ElapsedMilliseconds,
        context.Connection.RemoteIpAddress,
        _ipRequestCount[ipString]
        );
      }
      else if (response.StatusCode >= 400 && response.StatusCode < 500)
      {
        _logger.LogWarning("Date: {date}, Method: {Method} {Path}, StatusCode: {StatusCode}, in {ElapsedMilliseconds}ms IP address {IP} countRequest {count}",
        date,
        request.Method,
        request.Path,
        response.StatusCode,
        time.ElapsedMilliseconds,
        context.Connection.RemoteIpAddress,
        _ipRequestCount[ipString]
        );
      }
      else
      {
        _logger.LogError("Date: {date}, Method: {Method} {Path}, StatusCode: {StatusCode}, in {ElapsedMilliseconds}ms IP address {IP} countRequest {count}",
        date,
        request.Method,
        request.Path,
        response.StatusCode,
        time.ElapsedMilliseconds,
        context.Connection.RemoteIpAddress,
        _ipRequestCount[ipString]
        );
      }
    }
  }
}