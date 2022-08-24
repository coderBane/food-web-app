using System;
using Microsoft.AspNetCore.Identity;

namespace Foody.Entities.Models
{
    public class RefreshToken : BaseEntity
    {
        // user Id when logged in
        public string UserId { get; set; } = null!;

        public string Token { get; set; } = null!;

        // Id generated when jwt Id has been requested
        public string JwtId { get; set; } = null!;

        // make sure token is only used once
        public bool IsUsed { get; set; }

        // make sure they are valid
        public bool IsRevoked { get; set; }

        public DateTime ExipryDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;

    }
}

