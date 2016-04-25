using LicenseLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseLibrary
{
    public class ViolationDL
    {
        public static bool Save(Violation violation)
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    context.Violations.Add(violation);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Violation> RetrieveViolations()
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    var violations = context.Violations.ToList();
                    return violations;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
