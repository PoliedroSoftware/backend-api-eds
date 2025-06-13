using System.Text;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
       
        var requestLog = await FormatRequest(context.Request);
        var originalBodyStream = context.Response.Body;
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;
        await next(context);
        var responseLog = await FormatResponse(context.Response);
        await responseBody.CopyToAsync(originalBodyStream);
        var logData = new Dictionary<string, object>
        {
            ["Request"] = requestLog,
            ["Response"] = responseLog,
            ["TraceId"] = context.TraceIdentifier
        };

        logger.LogInformation("HTTP Request and Response: {@LogData}", logData);
    }

    private async Task<string> FormatRequest(HttpRequest request)
    {
        request.EnableBuffering();

        using var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        string body = await reader.ReadToEndAsync();

        request.Body.Position = 0;

        var builder = new StringBuilder();
        builder.AppendLine($"{request.Method} {request.Path}{request.QueryString}");
        builder.AppendLine($"Headers: {string.Join(", ", request.Headers)}");
        builder.AppendLine($"Body: {body}");

        return builder.ToString();
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);

        string text = await new StreamReader(response.Body).ReadToEndAsync();

        response.Body.Seek(0, SeekOrigin.Begin);

        var builder = new StringBuilder();
        builder.AppendLine($"Status Code: {response.StatusCode}");
        builder.AppendLine($"Headers: {string.Join(", ", response.Headers)}");
        builder.AppendLine($"Body: {text}");

        return builder.ToString();
    }
}

