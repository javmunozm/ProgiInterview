using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Domain.UnitTests;

/// <summary>
/// Fixed storage fee: always $100 regardless of price or type.
/// </summary>
public class StorageFeeTests
{
    private static decimal GetStorageFee(decimal price, VehicleType type) =>
        new BidCalculation(price, type).Fees.First(f => f.Name == "Storage").Amount;

    [Fact]
    public void Common_AlwaysReturns100()
    {
        GetStorageFee(999m, VehicleType.Common).Should().Be(100m);
    }

    [Fact]
    public void Luxury_AlwaysReturns100()
    {
        GetStorageFee(5000m, VehicleType.Luxury).Should().Be(100m);
    }

    [Fact]
    public void SmallPrice_AlwaysReturns100()
    {
        GetStorageFee(1m, VehicleType.Common).Should().Be(100m);
    }

    [Fact]
    public void LargePrice_AlwaysReturns100()
    {
        GetStorageFee(1000000m, VehicleType.Luxury).Should().Be(100m);
    }
}
