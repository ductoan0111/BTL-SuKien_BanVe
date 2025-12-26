using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities
{
    public class SuKien
    {
        public int SuKienId { get; set; }
        public string TenSuKien { get; set; } = null!;
        public string? MoTa { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public int DiaDiemId { get; set; }
        public int ToChucId { get; set; }   
        public bool TrangThai { get; set; }
    }
}
