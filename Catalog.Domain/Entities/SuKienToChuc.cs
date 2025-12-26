using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities
{
    public class SuKienToChuc
    {
        public int SuKienId { get; set; }
        public int NguoiDungId { get; set; } = 0; 
        public string VaiTroTrongSuKien { get; set; } = null;
        public DateTime NgayThamGia { get; set; }
    }
}
