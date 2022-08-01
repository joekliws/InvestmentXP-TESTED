using Investment.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }  = string.Empty;

        [Column(TypeName = "varchar(150)")]
        public string LastName { get; set; }  = string.Empty;

        [Column(TypeName = "varchar(50)")]
        public string? PreferedName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
        public RiskTolerance InvestorStyle { get; set; }

        [Column(TypeName = "char(11)")]
        public string Cpf { get; set; } = string.Empty;
        public bool Inactive => RemovedAt.HasValue;

        [Column(TypeName = "varbinary(128)")]
        public byte[] PasswordSalt { get; set; }

        [Column(TypeName = "varbinary(64)")]
        public byte[] PasswordHash { get; set; }
        public ICollection<UserAsset>? Assets { get; set; }

    }
}
