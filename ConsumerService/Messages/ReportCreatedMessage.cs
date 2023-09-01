namespace ConsumerService.Messages
{
    public class ReportCreatedMessage
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
