namespace Domain.ValueObjects;

public sealed record Fee(string Name, decimal Amount, string Rule, string Calculation);
