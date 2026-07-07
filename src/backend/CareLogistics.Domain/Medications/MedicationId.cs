using CSharpFunctionalExtensions;

namespace CareLogistics.Domain.Medications;

public readonly record struct MedicationId
{
    private MedicationId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public bool IsEmpty => Value == Guid.Empty;

    public static MedicationId New()
    {
        return new MedicationId(Guid.NewGuid());
    }

    public static Result<MedicationId> Create(Guid value)
    {
        return value == Guid.Empty
            ? Result.Failure<MedicationId>("Medication id cannot be empty.")
            : Result.Success(new MedicationId(value));
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
