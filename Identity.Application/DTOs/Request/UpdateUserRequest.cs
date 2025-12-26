using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.DTOs.Request
{
    public class UpdateUserRequest
    {
        public string FullName { get; set; } = null!;
        public int RoleId { get; set; }
        public bool TrangThai { get; set; }
    }
}
