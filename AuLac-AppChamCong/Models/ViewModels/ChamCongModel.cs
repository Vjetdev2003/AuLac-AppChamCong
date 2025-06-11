using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Models.ViewModels
{
    public class ChamCongModel
    {
        public decimal? MngChamCongPrkID { get; set; }
        public int? Buoi { get; set; }
        public int? ChamCong { get; set; }
        public DateTime NgayCham { get; set; }  
        public DateTime? NgayChinhSua { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc { get; set; }
        public string? UserWritePrkID { get; set; }
        public string? ComputerIP { get; set; }
        public string? ComputerName { get; set; }
        public int? Status { get; set; }
    }
}
