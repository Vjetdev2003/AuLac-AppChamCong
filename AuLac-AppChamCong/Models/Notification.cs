using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Models
{
    public class Notification
    {
        public string Message { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }  // Trạng thái đã đọc
    }
}
