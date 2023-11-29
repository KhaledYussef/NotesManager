using Core.Enums;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

using Pal.Services.FileManager;

using SendGrid;
using SendGrid.Helpers.Mail;

using Service.Helpers;

namespace Pal.Services.Email
{

    //-----------------------------------------------------------------------------------------------
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;
        private readonly SendGridAccountDetails _sendGridConfig;
 

        public EmailService(IConfiguration configuration)
        {

            _configuration = configuration;
            _sendGridConfig = _configuration.GetSection("AppSettings:SendGrid").Get<SendGridAccountDetails>();
        }

        public async Task<bool> SendEmail(string ToEmail, string ToName, string url, EmailType emailType, params object[] arg)
        {

            try
            {
                string fromEmail = "khaled@edaraat.com";
                string fromName = "Notes Manager";
                if (ToEmail == null || fromEmail == null)
                    return false;

                var client = new SendGridClient(_sendGridConfig.ApiKey);
                EmailAddress from = new(fromEmail, fromName);

                List<EmailAddress> tos = new()
                {
                    new EmailAddress(ToEmail, ToName)
                };

                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, "", "", "", false);
                EmailAddress replayTo = new(fromEmail, fromName);
                msg.SetReplyTo(replayTo);

                switch (emailType)
                {

                    case EmailType.ResetPassword:
                        msg.SetTemplateId(_sendGridConfig.ResetPasswordTemplate);
                        msg.SetTemplateData(new
                        {
                            url
                        });
                        var result = await client.SendEmailAsync(msg);
                        break;


                }

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}