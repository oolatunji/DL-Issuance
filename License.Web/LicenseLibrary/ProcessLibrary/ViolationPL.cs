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
                var allviolations = ViolationDL.RetrieveViolations();

                if (allviolations.Any())
                {
                    var cars = CardAccountRequestPL.RetrieveCardAccountRequests();

                    var violations = (from violation in allviolations
                                      select new ViolationDTO
                                      {
                                          LicenseID = violation.LicenseID,
                                          Lastname = cars.Where(x => x.LicenseID == violation.LicenseID).Select(x => x.Lastname).FirstOrDefault(),
                                          Othernames = string.Format("{0} {1}", cars.Where(x => x.LicenseID == violation.LicenseID).Select(x => x.FirstName).FirstOrDefault(), cars.Where(x => x.LicenseID == violation.LicenseID).Select(x => x.MiddleName).FirstOrDefault()),
                                          DateOfBirth = cars.Where(x => x.LicenseID == violation.LicenseID).Select(x => x.DateOfBirth).FirstOrDefault(),
                                          MaritalStatus = cars.Where(x => x.LicenseID == violation.LicenseID).Select(x => x.MaritalStatus).FirstOrDefault(),
                                          Sex = cars.Where(x => x.LicenseID == violation.LicenseID).Select(x => x.Sex).FirstOrDefault(),
                                          Religion = cars.Where(x => x.LicenseID == violation.LicenseID).Select(x => x.Religion).FirstOrDefault(),
                                          MothersMaidenName = cars.Where(x => x.LicenseID == violation.LicenseID).Select(x => x.MothersMaidenName).FirstOrDefault(),
                                          EmailAddress = cars.Where(x => x.LicenseID == violation.LicenseID).Select(x => x.EmailAddress).FirstOrDefault(),
                                          PhoneNumber = cars.Where(x => x.LicenseID == violation.LicenseID).Select(x => x.PhoneNumber).FirstOrDefault(),
                                          Address = cars.Where(x => x.LicenseID == violation.LicenseID).Select(x => x.Address).FirstOrDefault(),
                                          Nationality = cars.Where(x => x.LicenseID == violation.LicenseID).Select(x => x.Nationality).FirstOrDefault(),
                                          Details = violation.Details,
                                          Date = Convert.ToDateTime(violation.Date)
                                      }).ToList();

                    return violations;
                }
                else
                {
                    return new List<ViolationDTO>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
