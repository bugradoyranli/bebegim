using bebegim.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;
using bebegim.Data;
namespace bebegim.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger)
        {
            this._logger = _logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Hatayı logluyoruz (Debug yaparken kolaylık sağlar)
            _logger.LogError(exception, "Bir hata oluştu: {Message}", exception.Message);

            // Yanıt formatını ayarlıyoruz
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ErrorResponse
            {
                Success = false,
                Message = "İşlem başarısız oldu.",
                Detail = exception.Message // Canlıya geçerken burayı boş bırakabilirsin
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true; // Hatanın ele alındığını sisteme bildiriyoruz
        }
    }
}
