using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.Web
{
    public class Response
    {
        public string Result { get; set; }
        public long RecordID { get; set; }
        public string ErrMessage { get; set; } 
    }
}