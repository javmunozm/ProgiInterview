using Application.Queries.CalculateBid;
using Domain.Enums;
using FluentValidation;

namespace Application.Validators;

public sealed class CalculateBidQueryValidator : AbstractValidator<CalculateBidQuery>
{
    private const decimal MaxVehiclePrice = 1_000_000_000m;

    public CalculateBidQueryValidator()
    {
        RuleFor(x => x.VehiclePrice)
            .GreaterThan(0m)
            .WithMessage("Vehicle price must be greater than zero.")
            .LessThanOrEqualTo(MaxVehiclePrice)
            .WithMessage($"Vehicle price must not exceed {MaxVehiclePrice:N0}.");

        RuleFor(x => x.VehicleType)
            .NotEmpty()
            .WithMessage("Vehicle type is required.")
            .Must(IsValidVehicleType)
            .WithMessage("Vehicle type must be 'Common' or 'Luxury'.");
    }

    private static bool IsValidVehicleType(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        return Enum.GetNames<VehicleType>()
            .Any(name => string.Equals(name, trimmed, StringComparison.OrdinalIgnoreCase));
    }
}
