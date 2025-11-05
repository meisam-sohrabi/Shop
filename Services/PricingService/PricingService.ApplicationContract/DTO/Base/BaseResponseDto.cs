using System.Net;

namespace PricingService.ApplicationContract.DTO.Base
{
    public class BaseResponseDto<T>
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
        public IDictionary<string, string[]>? ValidationErrors { get; set; }
    }
}
