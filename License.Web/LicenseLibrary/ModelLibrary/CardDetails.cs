using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseLibrary
{
    public class CardDetails
    {
        public long ID { get; set; }
        public string CardNumber { get; set; }
        public bool Issued { get; set; }
        public string DateUploaded { get; set; }
        public long BranchID { get; set; }
        public string Branch { get; set; }
        public string response { get; set; }
    }
}
