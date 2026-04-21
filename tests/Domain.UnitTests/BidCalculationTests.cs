using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Domain.UnitTests;

/// <summary>
/// Verifies all 6 test cases from the Progi coding challenge specification.
/// </summary>
public class ChallengeTestCasesTests
{
    [Theory]
    [InlineData(398.00, VehicleType.Common, 39.80, 7.96, 5.00, 100.00, 550.76)]
    [InlineData(501.00, VehicleType.Common, 50.00, 10.02, 10.00, 100.00, 671.02)]
    [InlineData(57.00, VehicleType.Common, 10.00, 1.14, 5.00, 100.00, 173.14)]
    [InlineData(1800.00, VehicleType.Luxury, 180.00, 72.00, 15.00, 100.00, 2167.00)]
    [InlineData(1100.00, VehicleType.Common, 50.00, 22.00, 15.00, 100.00, 1287.00)]
    [InlineData(1000000.00, VehicleType.Luxury, 200.00, 40000.00, 20.00, 100.00, 1040320.00)]
    public void Calculate_ShouldMatchChallengeExpectedValues(
        decimal price,
        VehicleType type,
        decimal expectedBasic,
        decimal expectedSpecial,
        decimal expectedAssociation,
        decimal expectedStorage,
        decimal expectedTotal)
    {
        var result = new BidCalculation(price, type);

        var fees = result.Fees.ToDictionary(f => f.Name, f => f.Amount);

        fees["Basic"].Should().Be(expectedBasic);
        fees["Special"].Should().Be(expectedSpecial);
        fees["Association"].Should().Be(expectedAssociation);
        fees["Storage"].Should().Be(expectedStorage);
        result.TotalPrice.Should().Be(expectedTotal);
    }
}
