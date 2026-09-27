
using System.ComponentModel.DataAnnotations;

namespace CommuteMate.Core.DTOs.Auth;

public record RegisterRequest(
    [param: Required]
    [param: StringLength(200)]
    string FullName,

    [param: Required]
    [param: EmailAddress]
    [param: StringLength(200)]
    string Email,

    [param: Required]
    [param: Phone]
    [param: StringLength(50)]
    string Phone,

    [param: Required]
    [param: StringLength(100, MinimumLength = 6)]
    string Password
);

public record LoginRequest(
    [param: Required]
    [param: EmailAddress]
    string Email,

    [param: Required]
    string Password
);

public record RefreshTokenRequest(
    [param: Required]
    string RefreshToken
);

public record UserDto(
    int Id,

    [param: Required]
    [param: StringLength(200)]
    string FullName,

    [param: Required]
    [param: EmailAddress]
    [param: StringLength(200)]
    string Email,

    [param: Required]
    [param: Phone]
    [param: StringLength(50)]
    string Phone
);

public record AuthResponse(
    [param: Required]
    string AccessToken,

    [param: Required]
    string RefreshToken,

    int ExpiresInMinutes,

    [param: Required]
    UserDto User
);

