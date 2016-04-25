using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseLibrary
{
    public class FinePL
    {
        public static List<FineDTO> RetrieveFines()
        {
            try
            {
                var allfines = FineDL.RetrieveAllFines();

                if (allfines.Any())
                {
                    var cars = CardAccountRequestPL.RetrieveCardAccountRequests();

                    var fines = (from fine in allfines
                                      select new FineDTO
                                      {
                                          LicenseID = fine.LicenseID,
                                          Lastname = cars.Where(x => x.LicenseID == fine.LicenseID).Select(x => x.Lastname).FirstOrDefault(),
                                          Othernames = string.Format("{0} {1}", cars.Where(x => x.LicenseID == fine.LicenseID).Select(x => x.FirstName).FirstOrDefault(), cars.Where(x => x.LicenseID == fine.LicenseID).Select(x => x.MiddleName).FirstOrDefault()),
                                          DateOfBirth = cars.Where(x => x.LicenseID == fine.LicenseID).Select(x => x.DateOfBirth).FirstOrDefault(),
                                          MaritalStatus = cars.Where(x => x.LicenseID == fine.LicenseID).Select(x => x.MaritalStatus).FirstOrDefault(),
                                          Sex = cars.Where(x => x.LicenseID == fine.LicenseID).Select(x => x.Sex).FirstOrDefault(),
                                          Religion = cars.Where(x => x.LicenseID == fine.LicenseID).Select(x => x.Religion).FirstOrDefault(),
                                          MothersMaidenName = cars.Where(x => x.LicenseID == fine.LicenseID).Select(x => x.MothersMaidenName).FirstOrDefault(),
                                          EmailAddress = cars.Where(x => x.LicenseID == fine.LicenseID).Select(x => x.EmailAddress).FirstOrDefault(),
                                          PhoneNumber = cars.Where(x => x.LicenseID == fine.LicenseID).Select(x => x.PhoneNumber).FirstOrDefault(),
                                          Address = cars.Where(x => x.LicenseID == fine.LicenseID).Select(x => x.Address).FirstOrDefault(),
                                          Nationality = cars.Where(x => x.LicenseID == fine.LicenseID).Select(x => x.Nationality).FirstOrDefault(),
                                          Details = fine.Details,
                                          Date = Convert.ToDateTime(fine.Date)
                                      }).ToList();

                    return fines;
                }
                else
                {
                    return new List<FineDTO>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
