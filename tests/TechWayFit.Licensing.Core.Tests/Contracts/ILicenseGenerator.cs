using TechWayFit.Licensing.Core.Tests.Models;

namespace TechWayFit.Licensing.Core.Tests.Contracts;

public interface ILicenseGenerator
{
    License GenerateLicense(LicenseData licenseData);
    string GenerateLicenseJson(LicenseData licenseData);
    string SignLicense(LicenseData licenseData, string privateKey);
    SignedLicense GenerateAndSignLicense(LicenseData licenseData, string privateKey);
    bool ExportLicenseToFile(SignedLicense license, string filePath);
    KeyPair GenerateKeyPair(int keySize = 2048);
}
