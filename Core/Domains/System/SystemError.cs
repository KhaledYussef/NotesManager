namespace Core.Domains.System
{
    public class SystemError
    {
        public int Id { get; set; }
        public string Error { get; set; }
        public string StackTrace { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemError() { }
        public SystemError(Exception ex)
        {
            Error = ex.Message;
            StackTrace = ex.StackTrace;
            CreatedAt = DateTime.Now;
        }

        public SystemError(string error)
        {
            Error = error;
            CreatedAt = DateTime.Now;
        }
    }
}
