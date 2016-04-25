using LicenseLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseLibrary
{
    public class ViolationPL
    {
        public static bool Save(Violation violation, out string message)
        {
            try
            {
                var car = CardAccountRequestDL.GetCardAccountRequestByLicenseID(violation.LicenseID);

                if (car == null)
                {
                    message = string.Format("Invalid License ID: {0}", violation.LicenseID);
                    return false;
                }
                else if(car.ID == 0)
                {
                    message = string.Format("Invalid License ID: {0}", violation.LicenseID);
                    return false;
                }
                else
                {
                    message = string.Empty;
                    return ViolationDL.Save(violation);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ViolationDTO> RetrieveViolations()
        {
            try
            {
                return ViolationDL.RetrieveViolations();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
