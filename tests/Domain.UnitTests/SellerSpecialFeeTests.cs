using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Domain.UnitTests;

/// <summary>
/// Seller's special fee: Common = 2%, Luxury = 4%.
/// </summary>
public class SellerSpecialFeeTests
{
    private static decimal GetSpecialFee(decimal price, VehicleType type) =>
        new BidCalculation(price, type).Fees.First(f => f.Name == "Special").Amount;

    [Fact]
    public void Common_Returns2Percent()
    {
        GetSpecialFee(1000m, VehicleType.Common).Should().Be(20m);
    }

    [Fact]
    public void Common_SmallPrice_Returns2Percent()
    {
        GetSpecialFee(57m, VehicleType.Common).Should().Be(1.14m);
    }

    [Fact]
    public void Luxury_Returns4Percent()
    {
        GetSpecialFee(1800m, VehicleType.Luxury).Should().Be(72m);
    }

    [Fact]
    public void Luxury_LargePrice_Returns4Percent()
    {
        GetSpecialFee(1000000m, VehicleType.Luxury).Should().Be(40000m);
    }
}
