using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Models
{
    public class Account
    {
        public string UserPrkID { get; set; }
        public string UserID { get; set; }
        public string PsnPrkID { get; set; }
        public string PsnName { get; set; }
        public string PsnTypeID { get; set; }
        public string IsFullRightsHSBADT { get; set; }
        public string PsnTypeName { get; set; }
        public string DeptPrkID { get; set; }
        public string DeptName { get; set; }
    }
}
