using AuLac_AppChamCong.Models;
using AuLac_AppChamCong.Models.ToaDo;
using Syncfusion.Blazor.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Models.ToaDo
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ToaDo Data { get ; set; }
    }
}
