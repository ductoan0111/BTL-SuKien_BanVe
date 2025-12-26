using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities
{
    public class DiaDiem
    {
        public int DiaDiemId { get; set; }
        public string TenDiaDiem { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public int? SucChua { get; set; }        
        public string? MoTa { get; set; }
        public byte TrangThai { get; set; } = 1;
    }
}
