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

        public static List<ViolationDTO> RetrieveViolations()
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    var cars = CardAccountRequestDL.RetrieveCardAccountRequests();

                    var violations = (from violation in context.Violations
                                      select new ViolationDTO
                                      {
                                          LicenseID = violation.LicenseID,
                                          Lastname = cars.Where(x => x.LicenseID == violation.LicenseID).FirstOrDefault().Lastname,
                                          FirstName = cars.Where(x => x.LicenseID == violation.LicenseID).FirstOrDefault().FirstName,
                                          MiddleName = cars.Where(x => x.LicenseID == violation.LicenseID).FirstOrDefault().MiddleName,
                                          DateOfBirth = cars.Where(x => x.LicenseID == violation.LicenseID).FirstOrDefault().DateOfBirth,
                                          MaritalStatus = cars.Where(x => x.LicenseID == violation.LicenseID).FirstOrDefault().MaritalStatus,
                                          Sex = cars.Where(x => x.LicenseID == violation.LicenseID).FirstOrDefault().Sex,
                                          Religion = cars.Where(x => x.LicenseID == violation.LicenseID).FirstOrDefault().Religion,
                                          MothersMaidenName = cars.Where(x => x.LicenseID == violation.LicenseID).FirstOrDefault().MothersMaidenName,
                                          EmailAddress = cars.Where(x => x.LicenseID == violation.LicenseID).FirstOrDefault().EmailAddress,
                                          PhoneNumber = cars.Where(x => x.LicenseID == violation.LicenseID).FirstOrDefault().PhoneNumber,
                                          Address = cars.Where(x => x.LicenseID == violation.LicenseID).FirstOrDefault().Address.Replace(',',' '),
                                          Nationality = cars.Where(x => x.LicenseID == violation.LicenseID).FirstOrDefault().Nationality,
                                          Details = violation.Details,
                                          Date = Convert.ToDateTime(violation.Date)
                                      }).ToList();
                                            

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
