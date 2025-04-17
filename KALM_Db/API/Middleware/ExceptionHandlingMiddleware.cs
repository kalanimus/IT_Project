using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new
            {
                Error = ex.Error,
                Message = ex.Message,
                StatusCode = ex.StatusCode,
                Details = ex.Details
            });
        }
            
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"An error occurred: {ex.Message}");

                // Возвращение ответа с кодом 500
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Внутренняя ошибка сервера");
            }
        }
    }
}