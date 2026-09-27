using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using CommuteMate.Core.Interfaces;
using CommuteMate.Core.DTOs.Auth;
using CommuteMate.Core.Settings;
using CommuteMate.Core.Entities;
using CommuteMate.Core.Exceptions;
using BCrypt.Net;

namespace CommuteMate.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IRefreshTokenRepository _refreshTokens;
        private readonly CommuteMate.Core.Interfaces.ITokenService _tokenService;
        private readonly JwtSettings _jwt;

        public AuthService(IUserRepository users, IRefreshTokenRepository refreshTokens, CommuteMate.Core.Interfaces.ITokenService tokenService, IOptions<JwtSettings> jwt)
        {
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _refreshTokens = refreshTokens ?? throw new ArgumentNullException(nameof(refreshTokens));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _jwt = jwt?.Value ?? throw new ArgumentNullException(nameof(jwt));
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            if (await _users.EmailExistsAsync(request.Email, cancellationToken))
                throw new DuplicateEmailException();

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email.Trim(),
                Phone = request.Phone.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _users.AddAsync(user, cancellationToken);
            await _users.SaveChangesAsync(cancellationToken);

            return await GenerateTokensForUser(user, cancellationToken);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _users.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null) throw new InvalidCredentialsException();

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new InvalidCredentialsException();
            return await GenerateTokensForUser(user, cancellationToken);
        }

        public async Task<AuthResponse> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            var existing = await _refreshTokens.GetByTokenAsync(request.RefreshToken, cancellationToken);
            if (existing == null || existing.RevokedAtUtc != null || existing.ExpiryDateUtc <= DateTime.UtcNow)
                throw new InvalidCredentialsException("Invalid or expired refresh token");

            // revoke existing
            existing.RevokedAtUtc = DateTime.UtcNow;
            existing.Isrevoked = true;
            await _refreshTokens.SaveChangesAsync(cancellationToken);

            var user = await _users.GetByIdAsync(existing.UserId, cancellationToken);
            if (user == null) throw new ResourceNotFoundException("User not found for refresh token");

            return await GenerateTokensForUser(user, cancellationToken);
        }

        private async Task<AuthResponse> GenerateTokensForUser(User user, CancellationToken cancellationToken)
        {
            var (accessToken, refreshTokenString, accessExpiresAt) = await _tokenService.IssueTokensAsync(user, cancellationToken);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenString,
                ExpiryDateUtc = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDays)
            };

            await _refreshTokens.AddAsync(refreshToken, cancellationToken);
            await _refreshTokens.SaveChangesAsync(cancellationToken);

            return new AuthResponse(accessToken, refreshToken.Token, _jwt.AccessTokenMinutes, new UserDto(user.Id, user.FullName, user.Email, user.Phone));
        }

        // AuthService implements IAuthService via its public methods above.
    }
}
