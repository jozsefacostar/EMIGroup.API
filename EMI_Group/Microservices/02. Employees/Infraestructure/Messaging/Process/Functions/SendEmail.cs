using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Messageging.Process.Functions
{
    public class SendEmail
    {
        public void Send(string subject, string To, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Ing Jozsef", "ingjozsefacosta@gmail.com"));
            message.To.Add(new MailboxAddress("Dear Admin", To));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {

                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                client.Authenticate("ingjozsefacosta@gmail.com", "xvyzahhyhgxbmedq");
                client.Send(message);
                client.Disconnect(true);
                Console.WriteLine("Email sent successfully!");
            }
        }
    }
}