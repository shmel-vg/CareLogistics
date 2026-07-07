using CSharpFunctionalExtensions;

namespace CareLogistics.Domain.Medications;

public readonly record struct PhotoId
{
    private PhotoId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public bool IsEmpty => Value == Guid.Empty;

    public static PhotoId New()
    {
        return new PhotoId(Guid.NewGuid());
    }

    public static Result<PhotoId> Create(Guid value)
    {
        return value == Guid.Empty
            ? Result.Failure<PhotoId>("Photo id cannot be empty.")
            : Result.Success(new PhotoId(value));
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
