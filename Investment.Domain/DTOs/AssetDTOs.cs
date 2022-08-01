using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment.Domain.DTOs
{
    public class AssetCreateDTO
    {
        public int CodCliente { get; set; }
        public int CodAtivo { get; set; }
        public int QtdeAtivo { get; set; }
    }

    public class AssetReadDTO
    {
        public int CodAtivo { get; set; }
        public int QtdeAtivo { get; set; }
        public decimal Valor { get; set; }
    }

    public class CustomerAssetReadDTO : AssetReadDTO
    {
        public int CodCliente { get; set; }
    }
}
