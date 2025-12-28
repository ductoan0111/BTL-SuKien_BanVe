using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entities
{
    public class DonHang
    {
        public int DonHangID { get; set; }
        public int NguoiMuaID { get; set; }
        public int SuKienID { get; set; }
        public DateTime NgayDat { get; set; }
        public decimal TongTien { get; set; }
        public byte TrangThai { get; set; }
    }
}
