using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using TechWayFit.Licensing.Core.Tests.Contracts;
using TechWayFit.Licensing.Core.Tests.Models;

namespace TechWayFit.Licensing.Core.Tests.Services;

public class LicenseValidationService : ILicenseValidationService
{
    private readonly ILogger<LicenseValidationService> _logger;
    private readonly IMemoryCache _cache;

    public LicenseValidationService(ILogger<LicenseValidationService> logger, IMemoryCache cache)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<LicenseValidationResult> ValidateFromJsonAsync(string licenseJson, string publicKey)
    {
        if (string.IsNullOrEmpty(licenseJson))
        {
            throw new ArgumentNullException(nameof(licenseJson));
        }

        if (string.IsNullOrEmpty(publicKey))
        {
            throw new ArgumentException("Public key cannot be null or empty", nameof(publicKey));
        }

        // Try to parse the JSON to a License object
        try
        {
            var cacheKey = $"{licenseJson}_{publicKey}";

            // Check if we have a cached result
            if (_cache.TryGetValue(cacheKey, out LicenseValidationResult cachedResult))
            {
                _logger.LogInformation("Returning cached validation result");
                return cachedResult;
            }

            // In a real implementation, this would validate the signature
            // For testing purposes, we'll perform some basic validation
            await Task.Delay(10); // Simulate async work

            var result = new LicenseValidationResult { IsValid = false };

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var license = JsonSerializer.Deserialize<License>(licenseJson, options);

                if (license == null)
                {
                    result.ErrorMessage = "Failed to deserialize license";
                    return result;
                }

                result.License = license;

                // Check if license has expired
                if (license.ExpiresOn < DateTime.UtcNow)
                {
                    result.ErrorMessage = "License has expired";
                    return result;
                }

                // For test purposes, all validations will fail
                result.ErrorMessage = "This is a test implementation";

                // Cache the result
                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));

                return result;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing license JSON");
                result.ErrorMessage = "Invalid license format";
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error validating license");
            return new LicenseValidationResult
            {
                IsValid = false,
                ErrorMessage = $"Validation error: {ex.Message}"
            };
        }
    }
}
