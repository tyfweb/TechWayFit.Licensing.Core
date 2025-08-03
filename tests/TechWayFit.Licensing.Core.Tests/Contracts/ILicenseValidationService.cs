using TechWayFit.Licensing.Core.Tests.Models;

namespace TechWayFit.Licensing.Core.Tests.Contracts;

public interface ILicenseValidationService
{
    Task<LicenseValidationResult> ValidateFromJsonAsync(string licenseJson, string publicKey);
}

public class LicenseValidationResult
{
    public bool IsValid { get; set; }
    public License License { get; set; } = new License();
    public string? ErrorMessage { get; set; }
}
