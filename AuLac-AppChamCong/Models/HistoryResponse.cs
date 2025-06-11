using AuLac_AppChamCong.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Models
{
    public class HistoryResponse
    {
        public decimal MngChamCongPrkID { get; set; }
        public decimal PsnPrkID { get; set; }
        public int ThangCC { get; set; }
        public int NamCC { get; set; }
        public int NgayCC { get; set; }
        public List<ChamCongLineModel> Lines { get; set; } = new List<ChamCongLineModel>();
    }
}
