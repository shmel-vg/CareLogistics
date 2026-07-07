using CSharpFunctionalExtensions;

namespace CareLogistics.Domain.Patients;

public readonly record struct PatientId
{
    private PatientId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public bool IsEmpty => Value == Guid.Empty;

    public static PatientId New()
    {
        return new PatientId(Guid.NewGuid());
    }

    public static Result<PatientId> Create(Guid value)
    {
        return value == Guid.Empty
            ? Result.Failure<PatientId>("Patient id cannot be empty.")
            : Result.Success(new PatientId(value));
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
