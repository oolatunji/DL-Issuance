using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.Web.Models
{
    public class CardModel
    {
        public long Branch { get; set; }
        public string[] CardNumbers { get; set; }
    }
}