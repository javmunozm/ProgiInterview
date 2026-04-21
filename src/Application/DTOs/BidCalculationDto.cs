namespace Application.DTOs;

public sealed record FeeDto(string Name, decimal Amount, string Rule, string Calculation);

public sealed record BidCalculationDto(
    decimal VehiclePrice,
    string VehicleType,
    IReadOnlyList<FeeDto> Fees,
    decimal TotalPrice);
