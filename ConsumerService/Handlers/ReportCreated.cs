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
        await _emailService.SendEmailAsync(
            request.Email,
            "Report Created",
            $"Your report with ID {request.Id} has been created."
        );

        return Unit.Value;
    }
}
