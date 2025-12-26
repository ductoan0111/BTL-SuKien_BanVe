using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities
{
    public class Ve
    {
        public int VeId { get; set; }
        public int DonHangId { get; set; }
        public int SuKienId { get; set; }

        public string MaVe { get; set; } = null!;
        public string? GhiChu { get; set; }

        public byte TrangThaiVe { get; set; }    
        public byte TrangThai { get; set; }      
    }
}
