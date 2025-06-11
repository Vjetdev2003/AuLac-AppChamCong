using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Models
{
    public class HospitalDb
    {
        public int HospitalId { get; set; }
        public string HospitalName { get; set; }
        public string HospitalAddress { get; set; }
        public decimal? Latidude { get; set; }
        public decimal? Longidude { get;set; }
    }
}
