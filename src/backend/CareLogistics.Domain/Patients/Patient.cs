using CSharpFunctionalExtensions;

namespace CareLogistics.Domain.Patients;

public sealed class Patient
{
    private Patient()
    {
        FullName = default!;
        Email = default!;
    }

    private Patient(PatientId id, string fullName, string email)
    {
        Id = id;
        FullName = fullName;
        Email = email;
    }

    public PatientId Id { get; private set; }

    public string FullName { get; private set; }

    public string Email { get; private set; }

    public static Result<Patient> Create(string fullName, string email)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return Result.Failure<Patient>("Patient full name is required.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<Patient>("Patient email is required.");
        }

        return Result.Success(new Patient(PatientId.New(), fullName.Trim(), email.Trim()));
    }
}
