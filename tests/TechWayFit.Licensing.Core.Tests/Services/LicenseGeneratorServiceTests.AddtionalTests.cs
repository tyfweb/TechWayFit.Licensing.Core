using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using TechWayFit.Licensing.Core.Tests.Contracts;
using TechWayFit.Licensing.Core.Tests.Models;

namespace TechWayFit.Licensing.Core.Tests.Services;

public partial class LicenseGeneratorServiceTests
{
    [Fact]
    public void GenerateLicenseJson_WithNullInput_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.GenerateLicenseJson(null));
    }

    [Fact]
    public void SignLicense_WithNullInput_ThrowsArgumentNullException()
    {
        // Arrange
        var privateKey = "test-key";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.SignLicense(null, privateKey));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void SignLicense_WithInvalidPrivateKey_ThrowsArgumentException(string privateKey)
    {
        // Arrange
        var licenseData = new LicenseData
        {
            LicensedTo = "Test Company",
            ProductId = "PROD-001"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.SignLicense(licenseData, privateKey));
    }

    [Fact]
    public void GenerateAndSignLicense_WithNullInput_ThrowsArgumentNullException()
    {
        // Arrange
        var privateKey = "test-key";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.GenerateAndSignLicense(null, privateKey));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GenerateAndSignLicense_WithInvalidPrivateKey_ThrowsArgumentException(string privateKey)
    {
        // Arrange
        var licenseData = new LicenseData
        {
            LicensedTo = "Test Company",
            ProductId = "PROD-001"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.GenerateAndSignLicense(licenseData, privateKey));
    }

    [Fact]
    public void ExportLicenseToFile_WithNullLicense_ThrowsArgumentNullException()
    {
        // Arrange
        var filePath = "license.json";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.ExportLicenseToFile(null, filePath));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ExportLicenseToFile_WithInvalidFilePath_ThrowsArgumentException(string filePath)
    {
        // Arrange
        var signedLicense = new SignedLicense
        {
            License = new License
            {
                LicensedTo = "Test Company",
                ProductId = "PROD-001"
            },
            Signature = "test-signature"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.ExportLicenseToFile(signedLicense, filePath));
    }
}
