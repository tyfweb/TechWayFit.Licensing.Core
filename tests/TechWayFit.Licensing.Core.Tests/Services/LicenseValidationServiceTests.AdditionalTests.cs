using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using TechWayFit.Licensing.Core.Tests.Models;
using TechWayFit.Licensing.Core.Tests.Contracts;

namespace TechWayFit.Licensing.Core.Tests.Services;

public partial class LicenseValidationServiceTests
{
    [Fact]
    public async Task ValidateFromJsonAsync_WithMalformedJson_ReturnsInvalidResult()
    {
        // Arrange
        var licenseJson = "{malformed json"; 
        var publicKey = "test-public-key";

        // Act
        var result = await _validationService.ValidateFromJsonAsync(licenseJson, publicKey);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task ValidateFromJsonAsync_WithEmptyJson_ReturnsInvalidResult()
    {
        // Arrange
        var licenseJson = "{}"; 
        var publicKey = "test-public-key";

        // Act
        var result = await _validationService.ValidateFromJsonAsync(licenseJson, publicKey);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task ValidateFromJsonAsync_WithExpiredLicense_ReturnsInvalidResult()
    {
        // Arrange
        var licenseJson = "{\"licensedTo\":\"Test Company\",\"productId\":\"PROD-001\",\"expiresOn\":\"2020-01-01T00:00:00Z\",\"features\":[\"Feature1\"]}"; 
        var publicKey = "test-public-key";

        // Act
        var result = await _validationService.ValidateFromJsonAsync(licenseJson, publicKey);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsValid);
    }
}
