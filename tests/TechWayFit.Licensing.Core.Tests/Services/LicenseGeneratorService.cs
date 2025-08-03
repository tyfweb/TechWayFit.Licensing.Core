using Microsoft.Extensions.Logging;
using TechWayFit.Licensing.Core.Tests.Contracts;
using TechWayFit.Licensing.Core.Tests.Models;

namespace TechWayFit.Licensing.Core.Tests.Services;

public class LicenseGeneratorService
{
    private readonly ILicenseGenerator _licenseGenerator;
    private readonly ILogger<LicenseGeneratorService> _logger;

    public LicenseGeneratorService(ILicenseGenerator licenseGenerator, ILogger<LicenseGeneratorService> logger)
    {
        _licenseGenerator = licenseGenerator ?? throw new ArgumentNullException(nameof(licenseGenerator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public License GenerateLicense(LicenseData licenseData)
    {
        if (licenseData == null)
        {
            throw new ArgumentNullException(nameof(licenseData));
        }

        _logger.LogInformation("Generating license for {licensedTo}", licenseData.LicensedTo);
        return _licenseGenerator.GenerateLicense(licenseData);
    }

    public string GenerateLicenseJson(LicenseData licenseData)
    {
        if (licenseData == null)
        {
            throw new ArgumentNullException(nameof(licenseData));
        }

        _logger.LogInformation("Generating license JSON for {licensedTo}", licenseData.LicensedTo);
        return _licenseGenerator.GenerateLicenseJson(licenseData);
    }

    public string SignLicense(LicenseData licenseData, string privateKey)
    {
        if (licenseData == null)
        {
            throw new ArgumentNullException(nameof(licenseData));
        }

        if (string.IsNullOrEmpty(privateKey))
        {
            throw new ArgumentException("Private key cannot be null or empty", nameof(privateKey));
        }

        _logger.LogInformation("Signing license for {licensedTo}", licenseData.LicensedTo);
        return _licenseGenerator.SignLicense(licenseData, privateKey);
    }

    public SignedLicense GenerateAndSignLicense(LicenseData licenseData, string privateKey)
    {
        if (licenseData == null)
        {
            throw new ArgumentNullException(nameof(licenseData));
        }

        if (string.IsNullOrEmpty(privateKey))
        {
            throw new ArgumentException("Private key cannot be null or empty", nameof(privateKey));
        }

        _logger.LogInformation("Generating and signing license for {licensedTo}", licenseData.LicensedTo);
        return _licenseGenerator.GenerateAndSignLicense(licenseData, privateKey);
    }

    public bool ExportLicenseToFile(SignedLicense license, string filePath)
    {
        if (license == null)
        {
            throw new ArgumentNullException(nameof(license));
        }

        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
        }

        _logger.LogInformation("Exporting license to file {filePath}", filePath);
        return _licenseGenerator.ExportLicenseToFile(license, filePath);
    }

    public KeyPair GenerateKeyPair(int keySize = 2048)
    {
        if (keySize <= 0)
        {
            throw new ArgumentException("Key size must be greater than zero", nameof(keySize));
        }

        _logger.LogInformation("Generating key pair with size {keySize}", keySize);
        return _licenseGenerator.GenerateKeyPair(keySize);
    }
}
