using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Models
{
    public class LichLamViecDb
    {
        public int LichLamViecID { get; set; }
        public decimal? PsnPrkID { get; set; }
        public int LoaiCongViecID { get; set; }
        public string NoiDungChiTiet { get; set; }
        public DateTime NgayTruc { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public int PhanLoai { get; set; }
    }
}
