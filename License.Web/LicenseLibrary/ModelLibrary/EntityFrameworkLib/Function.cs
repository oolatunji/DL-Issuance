//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LicenseLibrary.ModelLibrary.EntityFrameworkLib
{
    using System;
    using System.Collections.Generic;
    
    public partial class Function
    {
        public Function()
        {
            this.RoleFunctions = new HashSet<RoleFunction>();
        }
    
        public long ID { get; set; }
        public string Name { get; set; }
        public string PageLink { get; set; }
        public string Status { get; set; }
    
        public virtual ICollection<RoleFunction> RoleFunctions { get; set; }
    }
}
