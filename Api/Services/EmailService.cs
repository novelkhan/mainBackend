using Api.DTOs.Account;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace Api.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public Task<bool> SendEmailAsync(EmailSendDto emailSend)
        {
            //MailjetClient client = new MailjetClient(_config["MailJet:ApiKey"], _config["MailJet:SecretKey"]);

            //var email = new TransactionalEmailBuilder()
            //     .WithFrom(new SendContact(_config["Email:From"], _config["Email:ApplicationName"]))
            //     .WithSubject(emailSend.Subject)
            //     .WithHtmlPart(emailSend.Body)
            //     .WithTo(new SendContact(emailSend.To))
            //     .Build();

            //var response = await client.SendTransactionalEmailAsync(email);
            //if (response.Messages != null)
            //{
            //    if (response.Messages[0].Status == "success")
            //    {
            //        return true;
            //    }
            //}

            //return false;







            


            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("data24host@gmail.com"));
            email.To.Add(MailboxAddress.Parse(emailSend.To));
            email.Subject = emailSend.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailSend.Body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("data24host@gmail.com", "buximvejccvacrcd");
            try
            {
                smtp.Send(email);
            }
            catch (System.Net.Mail.SmtpFailedRecipientException ex)
            {
                // ex.FailedRecipient and ex.GetBaseException() should give you enough info.
                ex.GetBaseException();
                smtp.Disconnect(true);
                return Task.FromResult(false);
            }
            smtp.Disconnect(true);

            return Task.FromResult(true);
        }
    }
}
