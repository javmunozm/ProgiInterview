using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Queries.CalculateBid;

public sealed class CalculateBidQueryHandler : IRequestHandler<CalculateBidQuery, BidCalculationDto>
{
    private readonly IMapper _mapper;

    public CalculateBidQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Task<BidCalculationDto> Handle(CalculateBidQuery request, CancellationToken cancellationToken)
    {
        var vehicleType = Enum.Parse<VehicleType>(request.VehicleType.Trim(), ignoreCase: true);
        var calculation = new BidCalculation(request.VehiclePrice, vehicleType);
        var dto = _mapper.Map<BidCalculationDto>(calculation);

        return Task.FromResult(dto);
    }
}
