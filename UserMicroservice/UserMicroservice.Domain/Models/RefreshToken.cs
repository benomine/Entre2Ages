using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserMicroservice.Domain.Models
{
    public class RefreshToken
    {
        [Key]
        public int TokenId { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }

        public virtual User User { get; set; }
    }
}