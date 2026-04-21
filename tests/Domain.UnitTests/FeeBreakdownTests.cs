using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Domain.UnitTests;

/// <summary>
/// Every fee must expose a non-empty rule and a calculation string so the UI
/// can render a per-segment breakdown.
/// </summary>
public class FeeBreakdownTests
{
    [Fact]
    public void EveryFee_HasRuleAndCalculation()
    {
        var calc = new BidCalculation(1000m, VehicleType.Common);

        calc.Fees.Should().OnlyContain(f =>
            !string.IsNullOrWhiteSpace(f.Rule) && !string.IsNullOrWhiteSpace(f.Calculation));
    }

    [Fact]
    public void BasicFee_Calculation_MentionsCap_WhenClampedToMax()
    {
        // 10% of 10000 = 1000 → capped at 50 (Common max)
        var calc = new BidCalculation(10_000m, VehicleType.Common);
        var basic = calc.Fees.Single(f => f.Name == "Basic");

        basic.Amount.Should().Be(50m);
        basic.Calculation.Should().Contain("capped").And.Contain("$50.00");
    }

    [Fact]
    public void BasicFee_Calculation_MentionsRaise_WhenClampedToMin()
    {
        // 10% of 50 = 5 → raised to 10 (Common min)
        var calc = new BidCalculation(50m, VehicleType.Common);
        var basic = calc.Fees.Single(f => f.Name == "Basic");

        basic.Amount.Should().Be(10m);
        basic.Calculation.Should().Contain("raised").And.Contain("$10.00");
    }

    [Fact]
    public void SpecialFee_Rule_MentionsVehicleType()
    {
        var calc = new BidCalculation(1000m, VehicleType.Luxury);
        var special = calc.Fees.Single(f => f.Name == "Special");

        special.Rule.Should().Contain("Luxury");
    }

    [Fact]
    public void AssociationFee_Calculation_MentionsTier()
    {
        var calc = new BidCalculation(2000m, VehicleType.Common);
        var association = calc.Fees.Single(f => f.Name == "Association");

        association.Amount.Should().Be(15m);
        association.Calculation.Should().Contain("$1,000").And.Contain("$3,000");
    }

    [Fact]
    public void StorageFee_Rule_IsFlat()
    {
        var calc = new BidCalculation(1000m, VehicleType.Common);
        var storage = calc.Fees.Single(f => f.Name == "Storage");

        storage.Amount.Should().Be(100m);
        storage.Rule.Should().Contain("Flat");
    }
}
