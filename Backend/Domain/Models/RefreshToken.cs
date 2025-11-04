using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = string.Empty;
        public string JwtId { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow;
        public bool IsUsed { get; set; } = false;
        public bool IsRevoked { get; set; } = false;
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
