using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment.Domain.Entities
{
    public class UserAsset
    {
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; } = new User();

        [ForeignKey("Asset")]
        public int AssetId { get; set; }

        public virtual Asset Asset { get; set; } = new Asset();

        public int Quantity { get; set; }

        public DateTime UtcBoughtAt { get; set; }

        public DateTime? UtcSoldAt { get; set; }
    }
}
