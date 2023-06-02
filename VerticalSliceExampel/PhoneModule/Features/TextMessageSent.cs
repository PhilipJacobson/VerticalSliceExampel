using MediatR;

namespace VerticalSliceExample.PhoneModule.Features;

public class TextMessageSent : INotification
{
    public Guid ReportId { get; set; }

    public class Handler : INotificationHandler<TextMessageSent>
    {
        private readonly ILogger<TextMessageSent> _logger;
        public Handler(ILogger<TextMessageSent> logger)
        {
            _logger = logger;
        }
        public async Task Handle(TextMessageSent notification, CancellationToken cancellationToken)
        {
             _logger.LogInformation($"Sending text to user based on reportId {notification.ReportId}");
        }
    }
}
