using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new LicenseService.ThirdPartyService();
            var enrolmentData = new LicenseService.EnrolmentModel
            {
                ID = 12,
                Lastname = "Wale",
                FirstName = "Onigbanjo",
                MiddleName = "Akanmu Ojo",
                NameOnCard = "Wale Onigbanjo",
                DateOfBirth = "01/10/2012",
                MaritalStatus = "Single",
                Sex = "Male",
                Religion = "Christianity",
                MothersMaidenName = "Gbemisola",
                Nationality = "Nigeria",
                UtilityBill = "Yes",
                IDNumber = "092839829",
                LocalGovernmentArea = "Ikeja",
                BloodGroup = "O+",
                LicenseType = "Car",
                IssueDate = "26/04/2016",
                ValidTillDate = "26/04/2018",
                FileNumber = "DL102901",
                EmailAddress = "wale.oni@gmail.com",
                PhoneNumber = "+89289282928",
                Address = "Ikeja, Lagos",
                UserPrinting = "webuser"
            };

            var result = service.EditData(enrolmentData);

            Console.WriteLine("Response:/nResult: {0}/nRecord ID: {1}/n Error Message: {2}", result.Result, result.RecordID, result.ErrMessage);


            //var count = service.GetCountofRecords("webuser");
            //Console.WriteLine(count);

            //var result = service.InsertFine("2642016132160", "Driving without a valid license");
            //Console.WriteLine("Response:/nResult: {0}/nRecord ID: {1}/n Error Message: {2}", result.Result, result.RecordID, result.ErrMessage);

            Console.Read();
        }
    }
}
