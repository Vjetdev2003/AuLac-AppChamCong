using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Models.DonCongTac
{
    public class DonCongTacResponse
    {
        public bool Success { get; set; }
        public List<DonCongTac> Data { get; set; }
    }
}
