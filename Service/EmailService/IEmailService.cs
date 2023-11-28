using Core.Enums;

namespace Pal.Services.Email
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string ToEmail, string ToName, string url, EmailType emailType, params object[] arg);
    }
}