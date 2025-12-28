using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities
{
    public class SuKien
    {
        public int SuKienID { get; set; }
        public int DanhMucID { get; set; }
        public int DiaDiemID { get; set; }
        public int ToChucID { get; set; }

        public string TenSuKien { get; set; } = string.Empty;
        public string? MoTa { get; set; }

        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }

        public string? AnhBiaUrl { get; set; }
        public byte TrangThai { get; set; } 
        public DateTime NgayTao { get; set; }
    }
}
