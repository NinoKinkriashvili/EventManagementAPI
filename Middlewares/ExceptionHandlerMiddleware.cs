    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using System.Text.Json;
namespace Pied_Piper.Middlewares
{
    
        public class ExceptionHandlerMiddleware
        {
            private readonly RequestDelegate _next;
            public ExceptionHandlerMiddleware(RequestDelegate next)
            {
                _next = next;
            }
            public async Task Invoke(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(context, ex);
                }
            }

            private async Task HandleExceptionAsync(HttpContext context, Exception ex)
            {
                var error = new ApiError(context, ex);
                var result = JsonSerializer.Serialize(error);
                context.Response.Clear();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = error.Status!.Value;

                await context.Response.WriteAsync(result);
            }



        }

        public class ApiError : ProblemDetails
        {
            public const string UnhandledError = "UnhandledError";
            private HttpContext _context;
            private Exception _exception;
            public string Code { get; set; }

            public string TraceId
            {
                get
                {
                    if (Extensions.TryGetValue("traceId", out var traceId))
                    {
                        return traceId?.ToString();
                    }
                    return null;
                }
                set
                {
                    Extensions["TraceId"] = value;
                }
            }

            public ApiError(HttpContext context, Exception exception)
            {
                _context = context;
                _exception = exception;

                TraceId = context.TraceIdentifier;
                Instance = context.Request.Path;

                //HandleException((dynamic)exception);
            }

            //private void HandleException(UserNotFoundException exception)
            //{
            //    Code = exception.Code;
            //    Status = (int)HttpStatusCode.NotFound;
            //    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            //    Title = exception.Message;
            //}
            //private void HandleException(UserAlreadyExistsException exception)
            //{
            //    Code = exception.Code;
            //    Status = (int)HttpStatusCode.Conflict;
            //    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.9";
            //    Title = exception.Message;
            //}

            //private void HandleException(ArgumentMismatchException exception)
            //{
            //    Code = exception.Code;
            //    Status = (int)HttpStatusCode.BadRequest;
            //    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
            //    Title = exception.Message;
            //}

            //private void HandleException(Exception exception)
            //{
            //    //Code =;
            //    Status = (int)HttpStatusCode.BadRequest;
            //    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
            //    Title = exception.Message;
            //}
        }
    

}
