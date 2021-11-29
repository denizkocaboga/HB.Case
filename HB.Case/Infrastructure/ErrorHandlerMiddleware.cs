using HB.Case.Api.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace HB.Case
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HbException e)
            {
                await FillResponseError(context, e.StatusCode, e.ResponseMessage);
            }
            catch (Exception e)
            {
                await FillResponseError(context, (int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private static async Task<HttpResponse> FillResponseError(HttpContext context, int statusCode, string message)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            if (!string.IsNullOrEmpty(message))
            {
                var result = JsonSerializer.Serialize(new { message });
                await response.WriteAsync(result);
            }

            return response;
        }
    }



}
