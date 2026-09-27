namespace CommuteMate.Core.Settings;

/// <summary>
/// Configuration settings for JWT (bound from configuration "Jwt" section).
/// </summary>
public class JwtSettings
{
    public const string SectionName = "Jwt";
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; } = 15;

    public int RefreshTokenDays { get; set; } = 7;
}
