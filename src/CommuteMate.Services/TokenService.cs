using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CommuteMate.Core.Interfaces;
using CommuteMate.Core.Settings;
using CommuteMate.Core.Entities;

namespace CommuteMate.Services
{
    internal class TokenService : ITokenService
    {
        private readonly JwtSettings _jwt;

        public TokenService(IOptions<JwtSettings> jwt)
        {
            _jwt = jwt?.Value ?? new JwtSettings();
        }

        // Create a JWT access token and return token + expiry
        public (string Token, DateTime ExpiresAtUtc) CreateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName)
            };

            var expires = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var written = new JwtSecurityTokenHandler().WriteToken(token);
            return (written, expires);
        }

        // Simple refresh token generator (GUID). Replace with secure random bytes in production.
        public string CreateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        public System.Threading.Tasks.Task<(string AccessToken, string RefreshToken, DateTime AccessExpiresAtUtc)> IssueTokensAsync(User user, System.Threading.CancellationToken cancellationToken = default)
        {
            var (token, expires) = CreateAccessToken(user);
            var refresh = CreateRefreshToken();
            return System.Threading.Tasks.Task.FromResult((token, refresh, expires));
        }
    }
}
