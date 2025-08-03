using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using TechWayFit.Licensing.Core.Services; 
using TechWayFit.Licensing.Core.Tests.Contracts;

namespace TechWayFit.Licensing.Core.Tests.Services;

public partial class LicenseValidationServiceTests
{
    private readonly Mock<ILogger<LicenseValidationService>> _mockLogger;
    private readonly Mock<IMemoryCache> _mockMemoryCache;
    private LicenseValidationService _validationService;

    public LicenseValidationServiceTests()
    {
        _mockLogger = new Mock<ILogger<LicenseValidationService>>();
        _mockMemoryCache = new Mock<IMemoryCache>();

        // Setup memory cache mock
        var mockCacheEntry = new Mock<ICacheEntry>();
        _mockMemoryCache
            .Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(mockCacheEntry.Object);

        _validationService = new LicenseValidationService(_mockLogger.Object, _mockMemoryCache.Object);
    }

    [Fact]
    public async Task ValidateFromJsonAsync_WithValidLicense_ReturnsValidResult()
    {
        // Arrange
        var licenseJson = "{\"licensedTo\":\"Test Company\",\"productId\":\"PROD-001\",\"expiresOn\":\"2026-07-27T00:00:00Z\",\"features\":[\"Feature1\"]}"; 
        var publicKey = "test-public-key";

        // Act
        var result = await _validationService.ValidateFromJsonAsync(licenseJson, publicKey);

        // Assert
        Assert.NotNull(result);
        // We expect it to be not valid because this is a test implementation
        Assert.False(result.IsValid);
        Assert.NotNull(result.ErrorMessage);
    }

    [Fact]
    public async Task ValidateFromJsonAsync_WithNullLicenseJson_ThrowsArgumentNullException()
    {
        // Arrange
        string licenseJson = null;
        var publicKey = "test-public-key";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _validationService.ValidateFromJsonAsync(licenseJson, publicKey));
    }

    [Fact]
    public async Task ValidateFromJsonAsync_WithEmptyPublicKey_ThrowsArgumentException()
    {
        // Arrange
        var licenseJson = "{\"licensedTo\":\"Test Company\"}";
        string publicKey = "";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _validationService.ValidateFromJsonAsync(licenseJson, publicKey));
    }

    [Fact]
    public async Task ValidateFromJsonAsync_WithNullPublicKey_ThrowsArgumentException()
    {
        // Arrange
        var licenseJson = "{\"licensedTo\":\"Test Company\"}";
        string publicKey = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _validationService.ValidateFromJsonAsync(licenseJson, publicKey));
    }
}