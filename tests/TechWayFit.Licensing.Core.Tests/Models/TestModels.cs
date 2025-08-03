namespace TechWayFit.Licensing.Core.Models;

// These model classes are added for testing purposes only
// They should match the actual model classes in the core project

public class LicenseData
{
    public string LicensedTo { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public DateTime ExpiresOn { get; set; } = DateTime.UtcNow.AddYears(1);
    public List<string> Features { get; set; } = new List<string>();
}

public class License
{
    public string LicensedTo { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public DateTime ExpiresOn { get; set; } = DateTime.UtcNow.AddYears(1);
    public List<string> Features { get; set; } = new List<string>();
}

public class SignedLicense
{
    public License License { get; set; } = new License();
    public string Signature { get; set; } = string.Empty;
}

public class KeyPair
{
    public string PrivateKey { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
}
