using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entities
{
    public class NguoiDung
    {
        public int NguoiDungID { get; set; }
        public string HoTen { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string MatKhauHash { get; set; } = null!;
        public int VaiTroID { get; set; }
        public DateTime NgayTao { get; set; }
        public bool TrangThai { get; set; }  
    }

}
