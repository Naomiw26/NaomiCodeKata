using System.Net;

namespace RPGCombatKata.Api
{
    public class BaseResponse
    {
        public bool Success => Array.Exists(new HttpStatusCode[2]
        {
            HttpStatusCode.OK,
            HttpStatusCode.Created
        }, (HttpStatusCode status) => status == Status);

        public HttpStatusCode Status { get; set; } = HttpStatusCode.NotFound;

        public string Message { get; set; }
    }
}
