using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseLibrary
{
    public class UserDetails
    {
        public long ID { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public List<FunctionModel> Function { get; set; }
        public string Response { get; set; }
    }
}
