using ConsumerService.Email;
using ConsumerService.Messages;
using MediatR;

namespace ConsumerService.Handlers;

public class ReportCreated : IRequest<ReportCreatedMessage>
{
    private readonly IEmailService _emailService;
    public ReportCreated(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<Unit> Handle(ReportCreatedMessage request, CancellationToken cancellationToken)
    {
        // Your logic for handling the report creation, if any

        // Send the email notification
        await _emailService.SendEmailAsync(
            request.Email,
            "Report Created",
            $"Your report with ID {request.Id} has been created."
        );

        return Unit.Value;
    }
}
