using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Exceptions;
using System.Net;

namespace RealEstateApp.WebApi.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            string exceptionTitle = "Internal server error";
            string details = exception.Message;

            switch (exception)
            {
                case ApiException e:
                    // custom application error
                    switch (e.ErrorCode)
                    {
                        case (int)HttpStatusCode.BadRequest:
                            exceptionTitle = "Bad request";                            
                            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        case (int)HttpStatusCode.InternalServerError:                           
                            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                        case (int)HttpStatusCode.NotFound:
                            exceptionTitle = "Not found";                           
                            httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                            break;
                        default:
                            // unhandled error                      
                            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;                           
                            break;
                    }
                    break;
                case ValidationException e:
                    // custom application error
                    exceptionTitle = "Bad request";
                    details = ((ValidationException)exception).Errors.Aggregate((a, b) => a + ", " + b);
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    exceptionTitle = "Not found";                  
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error                   
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var problemDetails = new ProblemDetails
            {
                Status = httpContext.Response.StatusCode,
                Title = exceptionTitle,
                Detail = details,
            };

            await httpContext.Response
               .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
