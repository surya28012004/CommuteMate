using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryDateUtc { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAtUtc { get; set; }

        public bool Isrevoked { get; set; }

        // Navigation
        public User? User { get; set; }
    }
}
