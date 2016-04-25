using LicenseLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseLibrary
{
    public class CardAccountRequestPL
    {
        public static bool Update(CardAccountRequest car)
        {
            try
            {
                return CardAccountRequestDL.Update(car);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CardAccountRequestDTO> RetrieveCardAccountRequests()
        {
            try
            {
                string key = System.Configuration.ConfigurationManager.AppSettings.Get("ekey");

                var cars = CardAccountRequestDL.RetrieveCardAccountRequests();

                if(cars.Any())
                {
                    var branchList = BranchDL.RetrieveBranches();

                    var result = (from car in cars
                                  select new CardAccountRequestDTO
                                 {
                                     ID = car.ID,
                                     LicenseID = car.LicenseID,
                                     CardSerialNumber = !string.IsNullOrEmpty(car.CardSerialNumber) ? Crypter.Decrypt(key, car.CardSerialNumber) : string.Empty,
                                     Lastname = car.Lastname,
                                     FirstName = car.FirstName,
                                     MiddleName = car.MiddleName,
                                     NameOnCard = car.NameOnCard,
                                     DateOfBirth = car.DateOfBirth,
                                     MaritalStatus = car.MaritalStatus,
                                     Sex = car.Sex,
                                     Religion = car.Religion,
                                     MothersMaidenName = car.MothersMaidenName,
                                     Nationality = car.Nationality,
                                     UtilityBill = car.UtilityBill,
                                     IDNumber = car.IDNumber,
                                     LocalGovernmentArea = car.LocalGovernmentArea,
                                     BloodGroup = car.BloodGroup,
                                     LicenseType = car.LicenseType,
                                     IssueDate = car.IssueDate,
                                     ValidTillDate = car.ValidTillDate,
                                     FileNumber = car.FileNumber,
                                     EmailAddress = car.EmailAddress,
                                     PhoneNumber = car.PhoneNumber,
                                     Address = car.Address,
                                     PrintStatus = car.PrintStatus == 1 ? "Enrolled" : (car.PrintStatus == 2 ? "SentForPrinting" : "Printed"),
                                     UserPrinting = car.UserPrinting,
                                     DateEnroled = Convert.ToDateTime(car.DateEnroled),
                                     DatePrinted = Convert.ToDateTime(car.DatePrinted),
                                     Branch = branchList.Where(x => x.ID == car.BranchID).FirstOrDefault().Name
                                 }).ToList();

                    return result;
                }
                else
                {
                    return new List<CardAccountRequestDTO>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
