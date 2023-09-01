using MediatR;
using VerticalSliceExample.CommonModule.Messaging;

namespace VerticalSliceExample.ReportModule.Features
{
    public class ReportCreated : INotification
    {
        public Guid Id { get; set; }

        public class Handler : INotificationHandler<ReportCreated>
        {
            private readonly MessagePublisher _messagePublisher;

            public Handler(MessagePublisher messagePublisher)
            {
                _messagePublisher = messagePublisher;
            }

            public async Task Handle(ReportCreated notification, CancellationToken cancellationToken)
            {
                //fetch the email addres assosicred with the report/organisation in question. 
                var email = "test@email.com";

                // Produce the message to RabbitMQ
                // Your logic here
                _ = _messagePublisher.PublishAsync(
                    new { Id = notification.Id, Email = email },
                    queueName: "ReportCreatedQueue" 
                    );
            }
        }
    }
}
