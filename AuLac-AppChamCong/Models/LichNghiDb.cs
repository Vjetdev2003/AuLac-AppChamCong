using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Models
{
    public class LichNghiDb
    {
        public int? LichNghiID { get; set; }
        public int? PsnPrkID { get; set; }
        public int LoaiNghiID { get; set; }
        public string? LoaiNghiName { get; set; }
        public DateTime NgayNopDon { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public string LyDo { get; set; }
        public int? Status { get; set; }
        public string? StatusDescription { get; set; }
        //  public string? CaLamViec { get; set; }  
    }
}