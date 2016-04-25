using LicenseLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseLibrary
{
    public class FineDL
    {
        public static bool Save(Fine fine, out long recordID)
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    context.Fines.Add(fine);
                    context.SaveChanges();
                }
                recordID = fine.ID;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Fine> RetrieveFines(string licenseID)
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    var fine = context.Fines
                                            .Where(f => f.LicenseID == licenseID);

                    return fine.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Fine> RetrieveAllFines()
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    var fine = context.Fines;

                    return fine.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
