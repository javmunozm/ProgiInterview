using Application.Queries.CalculateBid;
using Application.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests;

/// <summary>
/// Tests the FluentValidation rules for CalculateBidQuery.
/// </summary>
public class CalculateBidQueryValidatorTests
{
    private readonly CalculateBidQueryValidator _validator = new();

    [Fact]
    public void ValidQuery_Common_PassesValidation()
    {
        var query = new CalculateBidQuery(1000m, "Common");

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ValidQuery_Luxury_PassesValidation()
    {
        var query = new CalculateBidQuery(5000m, "Luxury");

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ValidQuery_CaseInsensitive_PassesValidation()
    {
        var query = new CalculateBidQuery(100m, "common");

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ZeroPrice_FailsValidation()
    {
        var query = new CalculateBidQuery(0m, "Common");

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.VehiclePrice);
    }

    [Fact]
    public void NegativePrice_FailsValidation()
    {
        var query = new CalculateBidQuery(-100m, "Common");

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.VehiclePrice);
    }

    [Fact]
    public void EmptyVehicleType_FailsValidation()
    {
        var query = new CalculateBidQuery(1000m, "");

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.VehicleType);
    }

    [Fact]
    public void InvalidVehicleType_FailsValidation()
    {
        var query = new CalculateBidQuery(1000m, "SUV");

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.VehicleType);
    }

    [Fact]
    public void NullVehicleType_FailsValidation()
    {
        var query = new CalculateBidQuery(1000m, null!);

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.VehicleType);
    }

    [Fact]
    public void NumericVehicleType_FailsValidation()
    {
        var query = new CalculateBidQuery(1000m, "0");

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.VehicleType);
    }

    [Fact]
    public void WhitespaceVehicleType_FailsValidation()
    {
        var query = new CalculateBidQuery(1000m, "   ");

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.VehicleType);
    }

    [Fact]
    public void VehicleType_WithSurroundingWhitespace_Passes()
    {
        var query = new CalculateBidQuery(1000m, "  Luxury  ");

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(x => x.VehicleType);
    }

    [Fact]
    public void AbsurdlyLargePrice_FailsValidation()
    {
        var query = new CalculateBidQuery(2_000_000_000m, "Common");

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.VehiclePrice);
    }
}
