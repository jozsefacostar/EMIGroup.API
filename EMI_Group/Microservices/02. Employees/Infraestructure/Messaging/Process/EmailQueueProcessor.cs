using Infraestructure.Messaging.Interface;
using Messageging.Process.Functions;

namespace Infraestructure.Messaging.Proccess
{
    public class EmailQueueProcessor : IQueueProcessor
    {
        private readonly SendEmail _sendEmail = new SendEmail();
        private readonly RabbitMQService _sendProcessBroker = new RabbitMQService();

        public EmailQueueProcessor()
        { }
        public EmailQueueProcessor(RabbitMQService sendProcessBroke)
        {
            _sendProcessBroker = sendProcessBroke;
        }
        public string QueueCode => "email_queue";
        public async Task ProcessMessageAsync(dynamic message)
        {

            EmailData? emailData = System.Text.Json.JsonSerializer.Deserialize<EmailData>(message.ToString());
            try
            {
                if (emailData != null)
                {
                    _sendEmail.Send(emailData.Subject, emailData.To, emailData.Body);
                    Console.WriteLine($"Processing Email: {emailData.Subject}");
                }
                else
                    Console.WriteLine("Error: EmailData deserialization failed.");
            }
            catch (Exception ex)
            {
                emailData.Body = $" Exception RE-Process :  {ex.ToString()} - \n\n {emailData.Body}";
                await _sendProcessBroker.SendMessageAsync(QueueCode, emailData);
            }

            await Task.CompletedTask;
        }





    }

    public class EmailData
    {
        public string? Subject { get; set; }
        public string? To { get; set; }
        public string? Body { get; set; }
    }
}
