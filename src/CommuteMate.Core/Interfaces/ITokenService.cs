using CommuteMate.Core.Entities;
using System;

namespace CommuteMate.Core.Interfaces
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAtUtc) CreateAccessToken(User user);
        string CreateRefreshToken();
        System.Threading.Tasks.Task<(string AccessToken, string RefreshToken, DateTime AccessExpiresAtUtc)> IssueTokensAsync(User user, System.Threading.CancellationToken cancellationToken = default);
    }
}
