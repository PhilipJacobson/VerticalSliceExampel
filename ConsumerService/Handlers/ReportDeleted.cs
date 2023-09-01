using ConsumerService.Email;
using ConsumerService.Messages;
using MediatR;

namespace ConsumerService.Handlers;

public class ReportDeleted :IRequest<ReportDeletedMessage>
{
    private readonly IEmailService _emailService;

    public ReportDeleted(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<Unit> Handle(ReportDeletedMessage request, CancellationToken cancellationToken)
    {
        await _emailService.SendEmailAsync(
            request.Email,
            "Report Deleted",
            $"Your report with ID {request.Id} has been deleted."
        );

        return Unit.Value;
    }
}   
