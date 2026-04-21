using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Domain.UnitTests;

/// <summary>
/// Basic buyer fee: 10% of the vehicle price.
/// Common: clamped between $10 and $50.
/// Luxury: clamped between $25 and $200.
/// </summary>
public class BasicBuyerFeeTests
{
    private static decimal GetBasicFee(decimal price, VehicleType type) =>
        new BidCalculation(price, type).Fees.First(f => f.Name == "Basic").Amount;

    [Fact]
    public void Common_BelowMinimum_ClampedTo10()
    {
        GetBasicFee(50m, VehicleType.Common).Should().Be(10m);
    }

    [Fact]
    public void Common_ExactMinimumThreshold_Returns10()
    {
        GetBasicFee(100m, VehicleType.Common).Should().Be(10m);
    }

    [Fact]
    public void Common_MidRange_Returns10Percent()
    {
        GetBasicFee(398m, VehicleType.Common).Should().Be(39.80m);
    }

    [Fact]
    public void Common_AtMaximumThreshold_Returns50()
    {
        GetBasicFee(500m, VehicleType.Common).Should().Be(50m);
    }

    [Fact]
    public void Common_AboveMaximum_ClampedTo50()
    {
        GetBasicFee(1000m, VehicleType.Common).Should().Be(50m);
    }

    [Fact]
    public void Luxury_BelowMinimum_ClampedTo25()
    {
        GetBasicFee(100m, VehicleType.Luxury).Should().Be(25m);
    }

    [Fact]
    public void Luxury_ExactMinimumThreshold_Returns25()
    {
        GetBasicFee(250m, VehicleType.Luxury).Should().Be(25m);
    }

    [Fact]
    public void Luxury_MidRange_Returns10Percent()
    {
        GetBasicFee(1800m, VehicleType.Luxury).Should().Be(180m);
    }

    [Fact]
    public void Luxury_AtMaximumThreshold_Returns200()
    {
        GetBasicFee(2000m, VehicleType.Luxury).Should().Be(200m);
    }

    [Fact]
    public void Luxury_AboveMaximum_ClampedTo200()
    {
        GetBasicFee(5000m, VehicleType.Luxury).Should().Be(200m);
    }
}
