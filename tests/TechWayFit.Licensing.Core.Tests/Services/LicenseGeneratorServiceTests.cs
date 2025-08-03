using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using TechWayFit.Licensing.Core.Models;

namespace TechWayFit.Licensing.Core.Tests.Services;

public class LicenseGeneratorServiceTests
{
    private readonly Mock<ILicenseGenerator> _mockLicenseGenerator;
    private readonly Mock<ILogger<LicenseGeneratorService>> _mockLogger;

    public LicenseGeneratorServiceTests()
    {
        _mockLicenseGenerator = new Mock<ILicenseGenerator>();
        _mockLogger = new Mock<ILogger<LicenseGeneratorService>>();
    }

    [Fact]
    public void GenerateLicense_WithValidInput_ReturnsLicenseObject()
    {
        // Arrange
        var licenseData = new LicenseData
        {
            LicensedTo = "Test Company",
            ProductId = "PROD-001",
            ExpiresOn = DateTime.UtcNow.AddYears(1),
            Features = new List<string> { "Feature1", "Feature2" }
        };

        var expectedLicense = new License
        {
            LicensedTo = "Test Company",
            ProductId = "PROD-001",
            ExpiresOn = licenseData.ExpiresOn,
            Features = licenseData.Features
        };

        _mockLicenseGenerator.Setup(lg => lg.GenerateLicense(It.IsAny<LicenseData>()))
            .Returns(expectedLicense);

        var service = new LicenseGeneratorService(_mockLicenseGenerator.Object, _mockLogger.Object);

        // Act
        var result = service.GenerateLicense(licenseData);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedLicense.LicensedTo, result.LicensedTo);
        Assert.Equal(expectedLicense.ProductId, result.ProductId);
        Assert.Equal(expectedLicense.ExpiresOn, result.ExpiresOn);
        Assert.Equal(expectedLicense.Features, result.Features);
    }

    [Fact]
    public void GenerateLicense_WithNullInput_ThrowsArgumentNullException()
    {
        // Arrange
        _mockLicenseGenerator.Setup(lg => lg.GenerateLicense(null))
            .Throws<ArgumentNullException>();

        var service = new LicenseGeneratorService(_mockLicenseGenerator.Object, _mockLogger.Object);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => service.GenerateLicense(null));
    }

    [Fact]
    public void GenerateLicenseJson_WithValidInput_ReturnsJsonString()
    {
        // Arrange
        var licenseData = new LicenseData
        {
            LicensedTo = "Test Company",
            ProductId = "PROD-001",
            ExpiresOn = DateTime.UtcNow.AddYears(1)
        };

        var expectedJson = "{\"licensedTo\":\"Test Company\",\"productId\":\"PROD-001\"}";

        _mockLicenseGenerator.Setup(lg => lg.GenerateLicenseJson(It.IsAny<LicenseData>()))
            .Returns(expectedJson);

        var service = new LicenseGeneratorService(_mockLicenseGenerator.Object, _mockLogger.Object);

        // Act
        var result = service.GenerateLicenseJson(licenseData);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedJson, result);
    }

    [Fact]
    public void SignLicense_WithValidInput_ReturnsSignedLicense()
    {
        // Arrange
        var licenseData = new LicenseData
        {
            LicensedTo = "Test Company",
            ProductId = "PROD-001",
            ExpiresOn = DateTime.UtcNow.AddYears(1)
        };

        var privateKey = "test-private-key";
        var expectedSignature = "signed-data-signature";

        _mockLicenseGenerator.Setup(lg => lg.SignLicense(It.IsAny<LicenseData>(), It.IsAny<string>()))
            .Returns(expectedSignature);

        var service = new LicenseGeneratorService(_mockLicenseGenerator.Object, _mockLogger.Object);

        // Act
        var result = service.SignLicense(licenseData, privateKey);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSignature, result);
    }

    [Fact]
    public void SignLicense_WithInvalidKey_ThrowsCryptographicException()
    {
        // Arrange
        var licenseData = new LicenseData
        {
            LicensedTo = "Test Company",
            ProductId = "PROD-001"
        };

        var invalidKey = "invalid-key";

        _mockLicenseGenerator.Setup(lg => lg.SignLicense(It.IsAny<LicenseData>(), invalidKey))
            .Throws<CryptographicException>();

        var service = new LicenseGeneratorService(_mockLicenseGenerator.Object, _mockLogger.Object);

        // Act & Assert
        Assert.Throws<CryptographicException>(() => service.SignLicense(licenseData, invalidKey));
    }

    [Fact]
    public void GenerateAndSignLicense_WithValidInput_ReturnsSignedLicenseObject()
    {
        // Arrange
        var licenseData = new LicenseData
        {
            LicensedTo = "Test Company",
            ProductId = "PROD-001",
            ExpiresOn = DateTime.UtcNow.AddYears(1)
        };

        var privateKey = "test-private-key";

        var expectedLicense = new SignedLicense
        {
            License = new License
            {
                LicensedTo = "Test Company",
                ProductId = "PROD-001",
                ExpiresOn = licenseData.ExpiresOn
            },
            Signature = "test-signature"
        };

        _mockLicenseGenerator.Setup(lg => lg.GenerateAndSignLicense(It.IsAny<LicenseData>(), It.IsAny<string>()))
            .Returns(expectedLicense);

        var service = new LicenseGeneratorService(_mockLicenseGenerator.Object, _mockLogger.Object);

        // Act
        var result = service.GenerateAndSignLicense(licenseData, privateKey);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedLicense.License.LicensedTo, result.License.LicensedTo);
        Assert.Equal(expectedLicense.License.ProductId, result.License.ProductId);
        Assert.Equal(expectedLicense.Signature, result.Signature);
    }

    [Fact]
    public void ExportLicenseToFile_WithValidInput_WritesToFileSystem()
    {
        // Arrange
        var signedLicense = new SignedLicense
        {
            License = new License
            {
                LicensedTo = "Test Company",
                ProductId = "PROD-001",
                ExpiresOn = DateTime.UtcNow.AddYears(1)
            },
            Signature = "test-signature"
        };

        var filePath = "license.json";
        _mockLicenseGenerator.Setup(lg => lg.ExportLicenseToFile(It.IsAny<SignedLicense>(), It.IsAny<string>()))
            .Returns(true);

        var service = new LicenseGeneratorService(_mockLicenseGenerator.Object, _mockLogger.Object);

        // Act
        var result = service.ExportLicenseToFile(signedLicense, filePath);

        // Assert
        Assert.True(result);
        _mockLicenseGenerator.Verify(lg => lg.ExportLicenseToFile(signedLicense, filePath), Times.Once);
    }

    [Fact]
    public void GenerateKeyPair_ReturnsValidKeyPair()
    {
        // Arrange
        var expectedKeyPair = new KeyPair
        {
            PrivateKey = "private-key-data",
            PublicKey = "public-key-data"
        };

        _mockLicenseGenerator.Setup(lg => lg.GenerateKeyPair(It.IsAny<int>()))
            .Returns(expectedKeyPair);

        var service = new LicenseGeneratorService(_mockLicenseGenerator.Object, _mockLogger.Object);

        // Act
        var result = service.GenerateKeyPair(2048);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedKeyPair.PrivateKey, result.PrivateKey);
        Assert.Equal(expectedKeyPair.PublicKey, result.PublicKey);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-512)]
    public void GenerateKeyPair_WithInvalidKeySize_ThrowsArgumentException(int keySize)
    {
        // Arrange
        _mockLicenseGenerator.Setup(lg => lg.GenerateKeyPair(keySize))
            .Throws<ArgumentException>();

        var service = new LicenseGeneratorService(_mockLicenseGenerator.Object, _mockLogger.Object);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.GenerateKeyPair(keySize));
    }
}
