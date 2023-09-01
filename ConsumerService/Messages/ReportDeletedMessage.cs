using MediatR;

namespace ConsumerService.Messages
{
    public class ReportDeletedMessage : IRequest
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
