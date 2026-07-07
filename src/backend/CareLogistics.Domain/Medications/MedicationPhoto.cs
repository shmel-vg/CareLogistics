using CSharpFunctionalExtensions;

namespace CareLogistics.Domain.Medications;

public sealed class MedicationPhoto
{
    private MedicationPhoto()
    {
        FilePath = default!;
    }

    private MedicationPhoto(PhotoId id, string filePath, bool isMain)
    {
        Id = id;
        FilePath = filePath;
        IsMain = isMain;
    }

    public PhotoId Id { get; private set; }

    public string FilePath { get; private set; }

    public bool IsMain { get; private set; }

    internal static Result<MedicationPhoto> Create(string filePath, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return Result.Failure<MedicationPhoto>("Photo file path is required.");
        }

        return Result.Success(new MedicationPhoto(PhotoId.New(), filePath.Trim(), isMain));
    }

    internal void MarkAsMain()
    {
        IsMain = true;
    }

    internal void MarkAsSecondary()
    {
        IsMain = false;
    }
}
