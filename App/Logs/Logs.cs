using System.Diagnostics;

namespace app.Logs;

public class Loggin
{
  private readonly RequestDelegate? _next;
  private readonly ILogger<Loggin> _logger;
  public Loggin(RequestDelegate next, ILogger<Loggin> logger)
  {
    _logger = logger;
    _next = next;
  }

  public async Task Invoke(HttpContext context)
  {
    var time = Stopwatch.StartNew();
    if (_next != null && _logger != null)
    {
      await _next(context);
      time.Stop();
      var request = context.Request;
      var response = context.Response;
      if (response.StatusCode < 400)
      {
        _logger.LogInformation("{Method} {Path} code {StatusCode} in {ElapsedMilliseconds}ms IP address {IP}",
        request.Method,
        request.Path,
        response.StatusCode,
        time.ElapsedMilliseconds,
        context.Connection.RemoteIpAddress
        );
      }
      else if (response.StatusCode >= 400 && response.StatusCode < 500)
      {
        _logger.LogWarning("{Method} {Path} code {StatusCode} in {ElapsedMilliseconds}ms IP address {IP}",
        request.Method,
        request.Path,
        response.StatusCode,
        time.ElapsedMilliseconds,
        context.Connection.RemoteIpAddress
        );
      }
      else
      {
        _logger.LogError("{Method} {Path} code {StatusCode} in {ElapsedMilliseconds}ms IP address {IP}",
        request.Method,
        request.Path,
        response.StatusCode,
        time.ElapsedMilliseconds,
        context.Connection.RemoteIpAddress
        );
      }
    }
  }
}