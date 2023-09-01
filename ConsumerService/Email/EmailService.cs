namespace ConsumerService.Email
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string body)
        {
            Console.WriteLine($"Sent email to {email} with subject {subject}");
            return Task.CompletedTask;
        }
    }
}
