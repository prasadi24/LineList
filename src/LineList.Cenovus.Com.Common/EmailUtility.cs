using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace LineList.Cenovus.Com.Common
{
    public class EmailUtility
    {
        private readonly IConfiguration _configuration;

        public EmailUtility(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool SendEmail(MailMessage message)
        {
            string senderAddress = _configuration["EmailSettings:SenderAddress"];
            string recipientAddress = _configuration["EmailSettings:RecipientAddress"];
            string serviceNowAddress = _configuration["EmailSettings:ServiceNowAddress"];
            string smtpServer = _configuration["EmailSettings:SmtpServer"];
            int smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            bool enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]);

            // Create the email message
            message.From = new MailAddress(senderAddress);
            message.To.Add(recipientAddress);
            message.CC.Add(serviceNowAddress);
            message.IsBodyHtml = true;

            // Send the email using SMTP
            SmtpClient smtpClient = new SmtpClient(smtpServer);
            smtpClient.Port = smtpPort;
            smtpClient.EnableSsl = enableSsl;

            smtpClient.Send(message);

            return true;
        }
    }
}