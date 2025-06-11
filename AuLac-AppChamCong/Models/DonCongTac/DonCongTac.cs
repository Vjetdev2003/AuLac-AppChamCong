using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Models.DonCongTac
{
    public class DonCongTac
    {
        public int MngDonCongTacPrkID { get; set; }
        public int PsnPrkID { get; set; }
        public DateTime NgayCongTac { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public int? HospitalId { get; set; }
        public string NoiCongTac { get; set; }
        public string LyDo { get; set; }
        public int? Status { get; set; }
        public string StatusDescription { get; set; }
    }
}
