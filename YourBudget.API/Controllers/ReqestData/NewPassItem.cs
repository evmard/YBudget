using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YourBudget.API.Controllers.ReqestData
{
    public class NewPassItem
    {
        public string OldPass { get; set; }
        public string NewPass { get; set; }
        public string ConfirmPass { get; set; }
    }

}
