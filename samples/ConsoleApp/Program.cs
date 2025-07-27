using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TechWayFit.Licensing.Core.Contracts;
using TechWayFit.Licensing.Core.Services;

var services = new ServiceCollection()
    .AddSingleton<ILicenseValidationService, LicenseValidationService>()
    .AddMemoryCache()
    .AddLogging(builder => builder.AddConsole())
    .BuildServiceProvider();

var validator = services.GetRequiredService<ILicenseValidationService>();

// Sample validation
var licenseJson = """{"licenseData":"sample","signature":"sample"}""";
var publicKey = "-----BEGIN PUBLIC KEY-----\nsample\n-----END PUBLIC KEY-----";

try
{
    var result = await validator.ValidateFromJsonAsync(licenseJson, publicKey);
    Console.WriteLine($"License Valid: {result.IsValid}");
    Console.WriteLine($"Status: {result.Status}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
