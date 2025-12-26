using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities
{
    public class LoaiVe
    {
        public int LoaiVeId { get; set; }
        public int SuKienId { get; set; }
        public string TenLoaiVe { get; set; } = null!;
        public decimal DonGia { get; set; }
        public int SoLuongToiDa { get; set; }
        public int? GioiHanMoiKhach { get; set; }
        public bool TrangThai { get; set; }
    }
}
