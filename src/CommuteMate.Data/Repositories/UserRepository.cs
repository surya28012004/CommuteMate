using System;
using System.Threading;
using System.Threading.Tasks;
using CommuteMate.Core.Entities;
using CommuteMate.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommuteMate.Data.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            ArgumentNullException.ThrowIfNull(db);
            _db = db;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email is required", nameof(email));
            var normalized = email.Trim();
            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.IsActive && u.Email == normalized, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.IsActive && u.Id == userId, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email is required", nameof(email));
            var normalized = email.Trim();
            return await _db.Users
                .AnyAsync(u => u.IsActive && u.Email == normalized, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> PhoneExistsAsync(string phone, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("phone is required", nameof(phone));
            var normalized = phone.Trim();
            return await _db.Users
                .AnyAsync(u => u.IsActive && u.Phone == normalized, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);
            await _db.Users.AddAsync(user, cancellationToken).ConfigureAwait(false);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _db.SaveChangesAsync(cancellationToken);
        }
    }
}
