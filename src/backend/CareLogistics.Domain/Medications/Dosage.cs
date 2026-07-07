using CSharpFunctionalExtensions;

namespace CareLogistics.Domain.Medications;

public readonly record struct Dosage
{
    private Dosage(decimal value, string unit)
    {
        Value = value;
        Unit = unit;
    }

    public decimal Value { get; }

    public string Unit { get; }

    public static Result<Dosage> Create(decimal value, string unit)
    {
        if (value <= 0)
        {
            return Result.Failure<Dosage>("Dosage value must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(unit))
        {
            return Result.Failure<Dosage>("Dosage unit is required.");
        }

        return Result.Success(new Dosage(value, unit.Trim()));
    }
}
