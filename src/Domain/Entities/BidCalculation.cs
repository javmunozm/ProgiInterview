using System.Globalization;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class BidCalculation
{
    private static readonly CultureInfo Money = CultureInfo.GetCultureInfo("en-US");

    public decimal VehiclePrice { get; }
    public VehicleType VehicleType { get; }
    public IReadOnlyList<Fee> Fees { get; }
    public decimal TotalPrice { get; }

    public BidCalculation(decimal vehiclePrice, VehicleType vehicleType)
    {
        VehiclePrice = vehiclePrice;
        VehicleType = vehicleType;

        Fees = new List<Fee>
        {
            BuildBasicBuyerFee(),
            BuildSellerSpecialFee(),
            BuildAssociationFee(),
            BuildStorageFee()
        };

        TotalPrice = vehiclePrice + Fees.Sum(f => f.Amount);
    }

    private Fee BuildBasicBuyerFee()
    {
        var (min, max, rate) = VehicleType switch
        {
            VehicleType.Common => (10m, 50m, 0.10m),
            VehicleType.Luxury => (25m, 200m, 0.10m),
            _ => throw new ArgumentOutOfRangeException(nameof(VehicleType), VehicleType, null)
        };

        var raw = VehiclePrice * rate;
        var amount = Math.Clamp(raw, min, max);
        var rule = $"{rate:P0} of base price (min {FormatMoney(min)}, max {FormatMoney(max)} for {VehicleType}).";
        var calculation = BuildClampCalculation(raw, amount, min, max, rate);

        return new Fee("Basic", amount, rule, calculation);
    }

    private Fee BuildSellerSpecialFee()
    {
        var rate = VehicleType switch
        {
            VehicleType.Common => 0.02m,
            VehicleType.Luxury => 0.04m,
            _ => throw new ArgumentOutOfRangeException(nameof(VehicleType), VehicleType, null)
        };

        var amount = VehiclePrice * rate;
        var rule = $"{rate:P0} of base price ({VehicleType}).";
        var calculation = $"{rate:P0} × {FormatMoney(VehiclePrice)} = {FormatMoney(amount)}";

        return new Fee("Special", amount, rule, calculation);
    }

    private Fee BuildAssociationFee()
    {
        var (amount, tier) = VehiclePrice switch
        {
            > 0m and <= 500m    => (5m,  "up to $500"),
            > 500m and <= 1000m => (10m, "$500–$1,000"),
            > 1000m and <= 3000m => (15m, "$1,000–$3,000"),
            > 3000m             => (20m, "over $3,000"),
            _                   => (0m,  "not applicable")
        };

        var rule = "Tiered fee based on base price ($5 / $10 / $15 / $20).";
        var calculation = $"Base price {FormatMoney(VehiclePrice)} falls in tier {tier} → {FormatMoney(amount)}";

        return new Fee("Association", amount, rule, calculation);
    }

    private static Fee BuildStorageFee()
    {
        const decimal amount = 100m;
        return new Fee("Storage", amount, "Flat storage fee.", $"Fixed {FormatMoney(amount)} regardless of price or type.");
    }

    private static string BuildClampCalculation(decimal raw, decimal final, decimal min, decimal max, decimal rate)
    {
        if (raw < min)
            return $"{rate:P0} × base = {FormatMoney(raw)} → raised to minimum {FormatMoney(min)}";
        if (raw > max)
            return $"{rate:P0} × base = {FormatMoney(raw)} → capped at maximum {FormatMoney(max)}";
        return $"{rate:P0} × base = {FormatMoney(final)} (within {FormatMoney(min)}–{FormatMoney(max)} band)";
    }

    private static string FormatMoney(decimal value) => value.ToString("C2", Money);
}
