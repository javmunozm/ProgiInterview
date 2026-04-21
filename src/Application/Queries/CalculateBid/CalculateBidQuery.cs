using Application.DTOs;
using MediatR;

namespace Application.Queries.CalculateBid;

public sealed record CalculateBidQuery(decimal VehiclePrice, string VehicleType) : IRequest<BidCalculationDto>;
