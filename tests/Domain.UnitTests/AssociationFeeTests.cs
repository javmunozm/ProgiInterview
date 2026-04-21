using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Domain.UnitTests;

/// <summary>
/// Association fee brackets:
///   $1–$500    → $5
///   $501–$1000 → $10
///   $1001–$3000→ $15
///   over $3000 → $20
/// </summary>
public class AssociationFeeTests
{
    private static decimal GetAssociationFee(decimal price) =>
        new BidCalculation(price, VehicleType.Common).Fees.First(f => f.Name == "Association").Amount;

    [Theory]
    [InlineData(0.01, 5)]
    [InlineData(0.50, 5)]
    [InlineData(1, 5)]
    [InlineData(250, 5)]
    [InlineData(500, 5)]
    public void Bracket1_UpTo500_Returns5(decimal price, decimal expected)
    {
        GetAssociationFee(price).Should().Be(expected);
    }

    [Theory]
    [InlineData(501, 10)]
    [InlineData(750, 10)]
    [InlineData(1000, 10)]
    public void Bracket2_501To1000_Returns10(decimal price, decimal expected)
    {
        GetAssociationFee(price).Should().Be(expected);
    }

    [Theory]
    [InlineData(1001, 15)]
    [InlineData(2000, 15)]
    [InlineData(3000, 15)]
    public void Bracket3_1001To3000_Returns15(decimal price, decimal expected)
    {
        GetAssociationFee(price).Should().Be(expected);
    }

    [Theory]
    [InlineData(3001, 20)]
    [InlineData(10000, 20)]
    [InlineData(1000000, 20)]
    public void Bracket4_Over3000_Returns20(decimal price, decimal expected)
    {
        GetAssociationFee(price).Should().Be(expected);
    }
}
