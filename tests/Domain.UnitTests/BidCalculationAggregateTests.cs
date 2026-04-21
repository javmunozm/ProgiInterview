using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Domain.UnitTests;

/// <summary>
/// Tests the BidCalculation aggregate root: fee count, total formula, and properties.
/// </summary>
public class BidCalculationAggregateTests
{
    [Fact]
    public void Always_Returns4Fees()
    {
        var result = new BidCalculation(1000m, VehicleType.Common);

        result.Fees.Should().HaveCount(4);
    }

    [Fact]
    public void FeeNames_AreBasicSpecialAssociationStorage()
    {
        var result = new BidCalculation(1000m, VehicleType.Common);

        result.Fees.Select(f => f.Name)
            .Should().ContainInOrder("Basic", "Special", "Association", "Storage");
    }

    [Fact]
    public void TotalPrice_EqualsPricePlusSumOfFees()
    {
        var result = new BidCalculation(500m, VehicleType.Common);

        var expectedTotal = result.VehiclePrice + result.Fees.Sum(f => f.Amount);
        result.TotalPrice.Should().Be(expectedTotal);
    }

    [Fact]
    public void VehiclePrice_PreservesInput()
    {
        var result = new BidCalculation(1234.56m, VehicleType.Luxury);

        result.VehiclePrice.Should().Be(1234.56m);
    }

    [Fact]
    public void VehicleType_PreservesInput()
    {
        var result = new BidCalculation(100m, VehicleType.Luxury);

        result.VehicleType.Should().Be(VehicleType.Luxury);
    }
}
