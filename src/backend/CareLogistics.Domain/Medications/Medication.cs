using CareLogistics.Domain.Patients;
using CSharpFunctionalExtensions;

namespace CareLogistics.Domain.Medications;

public sealed class Medication
{
    private readonly List<MedicationPhoto> _photos = [];

    private Medication()
    {
        Name = default!;
        ActiveSubstance = default!;
    }

    private Medication(
        MedicationId id,
        PatientId patientId,
        string name,
        string activeSubstance,
        MedicationForm form,
        Dosage dosage,
        bool isPrescriptionRequired,
        DateOnly expirationDate,
        int remainingQuantity,
        DateTimeOffset createdAt)
    {
        Id = id;
        PatientId = patientId;
        Name = name;
        ActiveSubstance = activeSubstance;
        Form = form;
        Dosage = dosage;
        IsPrescriptionRequired = isPrescriptionRequired;
        ExpirationDate = expirationDate;
        RemainingQuantity = remainingQuantity;
        CreatedAt = createdAt;
    }

    public MedicationId Id { get; private set; }

    public PatientId PatientId { get; private set; }

    public string Name { get; private set; }

    public string ActiveSubstance { get; private set; }

    public MedicationForm Form { get; private set; }

    public Dosage Dosage { get; private set; }

    public bool IsPrescriptionRequired { get; private set; }

    public DateOnly ExpirationDate { get; private set; }

    public int RemainingQuantity { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public IReadOnlyCollection<MedicationPhoto> Photos => _photos.AsReadOnly();

    public static Result<Medication> Create(
        PatientId patientId,
        string name,
        string activeSubstance,
        MedicationForm form,
        Dosage dosage,
        bool isPrescriptionRequired,
        DateOnly expirationDate,
        int remainingQuantity,
        DateTimeOffset createdAt)
    {
        if (patientId.IsEmpty)
        {
            return Result.Failure<Medication>("Patient id is required.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Medication>("Medication name is required.");
        }

        if (string.IsNullOrWhiteSpace(activeSubstance))
        {
            return Result.Failure<Medication>("Medication active substance is required.");
        }

        if (!Enum.IsDefined(form))
        {
            return Result.Failure<Medication>("Medication form is invalid.");
        }

        if (dosage.Value <= 0 || string.IsNullOrWhiteSpace(dosage.Unit))
        {
            return Result.Failure<Medication>("Medication dosage is invalid.");
        }

        if (expirationDate == default)
        {
            return Result.Failure<Medication>("Medication expiration date is required.");
        }

        if (remainingQuantity < 0)
        {
            return Result.Failure<Medication>("Medication remaining quantity cannot be negative.");
        }

        if (createdAt == default)
        {
            return Result.Failure<Medication>("Medication creation date is required.");
        }

        var medication = new Medication(
            MedicationId.New(),
            patientId,
            name.Trim(),
            activeSubstance.Trim(),
            form,
            dosage,
            isPrescriptionRequired,
            expirationDate,
            remainingQuantity,
            createdAt);

        return Result.Success(medication);
    }

    public Result<MedicationPhoto> AddPhoto(string filePath, bool isMain = false)
    {
        var photoResult = MedicationPhoto.Create(filePath, isMain);

        if (photoResult.IsFailure)
        {
            return Result.Failure<MedicationPhoto>(photoResult.Error);
        }

        var photo = photoResult.Value;

        if (isMain)
        {
            ClearMainPhoto();
        }

        _photos.Add(photo);

        return Result.Success(photo);
    }

    public Result SetMainPhoto(PhotoId photoId)
    {
        if (photoId.IsEmpty)
        {
            return Result.Failure("Photo id is required.");
        }

        var photo = _photos.SingleOrDefault(currentPhoto => currentPhoto.Id == photoId);

        if (photo is null)
        {
            return Result.Failure("Medication photo was not found.");
        }

        ClearMainPhoto();
        photo.MarkAsMain();

        return Result.Success();
    }

    public Result UpdateRemainingQuantity(int remainingQuantity)
    {
        if (remainingQuantity < 0)
        {
            return Result.Failure("Medication remaining quantity cannot be negative.");
        }

        RemainingQuantity = remainingQuantity;

        return Result.Success();
    }

    private void ClearMainPhoto()
    {
        foreach (var photo in _photos)
        {
            photo.MarkAsSecondary();
        }
    }
}
