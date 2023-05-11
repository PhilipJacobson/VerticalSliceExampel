using AutoMapper.Execution;
using MediatR;
using VerticalSliceExample.CommonModule;
using VerticalSliceExample.PhoneModule.Models.ViewModels;

namespace VerticalSliceExample.PhoneModule.Features;

public class GetPhone : IRequest<IResponse<Phone>>
{
    public class Handler : IRequestHandler<GetPhone, IResponse<Phone>>
    {
        public Handler()
        {
        }
        public async Task<IResponse<Phone>> Handle(GetPhone request, CancellationToken cancellationToken)
        {
           await Task.Delay(500);
           return Response<Phone>.Ok(new Phone() { Number = "07038383883" });
        }
    }
}
