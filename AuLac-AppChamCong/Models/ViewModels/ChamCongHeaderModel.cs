using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Models.ViewModels
{
    public class ChamCongHeaderModel
    {
        public decimal? MngChamCongPrkID { get; set; }
        public decimal? PsnPrkID { get; set; }
        public int ThangCC { get; set; }
        public int NamCC { get; set; }
        public int NgayCC { get; set; }
    }
}
