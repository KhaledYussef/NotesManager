using System.Net;

namespace Core.Domains
{
    public class ResponseResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> Errors { get; set; }
        public object Data { get; set; }
    }
}
