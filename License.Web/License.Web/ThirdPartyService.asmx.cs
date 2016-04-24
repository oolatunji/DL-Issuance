using License.Web.Models;
using LicenseLibrary;
using LicenseLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Services;

namespace License.Web
{
    /// <summary>
    /// Summary description for ThirdPartyService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ThirdPartyService : System.Web.Services.WebService
    {

        [WebMethod]
        public CardDetails RetrieveCardDetails(string cardNumber)
        {
            var pd = new CardDetails();
            try
            {
                if (string.IsNullOrEmpty(cardNumber))
                {
                    pd.response = String.Format("{0}|{1}", "Failed", "Pan has no value.");
                }
                else
                {
                    string hashed_pan = PasswordHash.MD5Hash(cardNumber);

                    var details = CardPL.RetrievePregeneratedCard(hashed_pan);

                    if (details == null)
                    {
                        pd.response = String.Format("{0}|{1}", "Failed", "Pan does not exist.");
                    }
                    else
                    {
                        pd = details;
                        pd.response = String.Format("{0}|{1}", "Success", "Pan exists.");
                    }
                }
            }
            catch (Exception ex)
            {
                pd.response = String.Format("{0}|{1}", "Failed", ex.Message);
                ErrorHandler.WriteError(ex);
            }

            return pd;
        }

        [WebMethod]
        public CardDetails GetCardSerialNumber()
        {
            var pd = new CardDetails();
            try
            {

                var details = CardPL.GetCardSerialNumber();

                if (details == null)
                {
                    pd.response = String.Format("{0}|{1}", "Failed", "No available card serial number.");
                }
                else
                {
                    pd = details;
                    pd.response = String.Format("{0}|{1}", "Success", "Card serial number exists.");
                }
            }
            catch (Exception ex)
            {
                pd.response = String.Format("{0}|{1}", "Failed", ex.Message);
                ErrorHandler.WriteError(ex);
            }

            return pd;
        }

        [WebMethod]
        public CardDetails[] RetrieveAvailableCards()
        {
            try
            {
                return CardPL.RetrieveAvailablePregeneratedCards().ToArray();
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                throw ex;
            }
        }

        [WebMethod]
        public UserDetails AuthenticateUser(string key, string key_pd)
        {
            var msg = string.Empty;
            var user = new UserDetails();
            try
            {
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(key_pd))
                {
                    user.Response = String.Format("{0}|{1}", "Failed", "All the parameters compulsory.");
                }
                else
                {
                    string password = PasswordHash.MD5Hash(key_pd);
                    string username = key;

                    var details = UserPL.AuthenticateUser(username, password);

                    if (details == null)
                    {
                        user.Response = String.Format("{0}|{1}", "Failed", "Validation of user failed");
                    }
                    else
                    {
                        user = details;
                        user.Response = String.Format("{0}|{1}", "Success", "Validation of user is successful");
                    }
                }
            }
            catch (Exception ex)
            {
                user.Response = String.Format("{0}|{1}", "Failed", ex.Message);
                ErrorHandler.WriteError(ex);
            }

            return user;
        }

        [WebMethod]
        public Response SaveData (EnrolmentModel enrolmentData)
        {
            try
            {
                var car = new CardAccountRequest
                {
                    Lastname = enrolmentData.Lastname,
                    FirstName = enrolmentData.FirstName,
                    MiddleName = enrolmentData.MiddleName,
                    NameOnCard = enrolmentData.NameOnCard,
                    DateOfBirth = enrolmentData.DateOfBirth,
                    MaritalStatus = enrolmentData.MaritalStatus,
                    Sex = enrolmentData.Sex,
                    Religion = enrolmentData.Religion,
                    MothersMaidenName = enrolmentData.MothersMaidenName,
                    Nationality = enrolmentData.Nationality,
                    UtilityBill = enrolmentData.UtilityBill,
                    IDNumber = enrolmentData.IDNumber,
                    LocalGovernmentArea= enrolmentData.LocalGovernmentArea,
                    BloodGroup = enrolmentData.BloodGroup,
                    LicenseType = enrolmentData.LicenseType,
                    IssueDate = enrolmentData.IssueDate,
                    ValidTillDate = enrolmentData.ValidTillDate,
                    FileNumber = enrolmentData.FileNumber,
                    EmailAddress = enrolmentData.EmailAddress,
                    PhoneNumber = enrolmentData.PhoneNumber,
                    Address = enrolmentData.Address,
                    Photo = enrolmentData.Photo,
                    FingerIdLeft = enrolmentData.FingerIdLeft,
                    FingerPrintLeft = enrolmentData.FingerPrintLeft,
                    FingerIdRight = enrolmentData.FingerIdRight,
                    FingerPrintRight = enrolmentData.FingerPrintRight,
                    PrintStatus = 1,
                    UserPrinting = enrolmentData.UserPrinting,
                    DateEnroled = System.DateTime.Now,
                    LicenseID = String.Format("{0:dMyyyyHHmmss}", System.DateTime.Now)
                };

                long recordID = 0;
                bool saved = CardAccountRequestDL.Save(car, out recordID);
                if(saved)
                {
                    return new Response
                    {
                        Result = "Success",
                        RecordID = recordID,
                    };
                }
                else
                {
                    return new Response
                    {
                        Result = "Failed",
                        RecordID = 0,
                        ErrMessage = "Operation Failed"
                    };
                }
            }
            catch(Exception ex)
            {
                return new Response
                {
                    Result = "Failed",
                    RecordID = 0,
                    ErrMessage = ex.Message
                };
            }
        }

        [WebMethod]
        public Response EditData(EnrolmentModel enrolmentData)
        {
            try
            {
                var car = new CardAccountRequest
                {
                    ID = enrolmentData.ID,
                    Lastname = enrolmentData.Lastname,
                    FirstName = enrolmentData.FirstName,
                    MiddleName = enrolmentData.MiddleName,
                    NameOnCard = enrolmentData.NameOnCard,
                    DateOfBirth = enrolmentData.DateOfBirth,
                    MaritalStatus = enrolmentData.MaritalStatus,
                    Sex = enrolmentData.Sex,
                    Religion = enrolmentData.Religion,
                    MothersMaidenName = enrolmentData.MothersMaidenName,
                    Nationality = enrolmentData.Nationality,
                    UtilityBill = enrolmentData.UtilityBill,
                    IDNumber = enrolmentData.IDNumber,
                    LocalGovernmentArea = enrolmentData.LocalGovernmentArea,
                    BloodGroup = enrolmentData.BloodGroup,
                    LicenseType = enrolmentData.LicenseType,
                    IssueDate = enrolmentData.IssueDate,
                    ValidTillDate = enrolmentData.ValidTillDate,
                    FileNumber = enrolmentData.FileNumber,
                    EmailAddress = enrolmentData.EmailAddress,
                    PhoneNumber = enrolmentData.PhoneNumber,
                    Address = enrolmentData.Address,
                    Photo = enrolmentData.Photo,
                    FingerIdLeft = enrolmentData.FingerIdLeft,
                    FingerPrintLeft = enrolmentData.FingerPrintLeft,
                    FingerIdRight = enrolmentData.FingerIdRight,
                    FingerPrintRight = enrolmentData.FingerPrintRight,
                };

                long recordID = 0;
                bool saved = CardAccountRequestDL.Save(car, out recordID);
                if (saved)
                {
                    return new Response
                    {
                        Result = "Success",
                        RecordID = recordID,
                    };
                }
                else
                {
                    return new Response
                    {
                        Result = "Failed",
                        RecordID = 0,
                        ErrMessage = "Operation Failed"
                    };
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Result = "Failed",
                    RecordID = 0,
                    ErrMessage = ex.Message
                };
            }
        }

        [WebMethod]
        public long GetCountofRecords()
        {
            try
            {
                return CardAccountRequestDL.RecordsToBePrinted();
            }
            catch(Exception ex)
            {
                ErrorHandler.WriteError(ex);
                throw ex;
            }
        }

        [WebMethod]
        public CardAccountRequest GetFullDataWithID(long ID)
        {
            try
            {
                return CardAccountRequestDL.GetFullDataWithID(ID);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                throw ex;
            }
        }

        [WebMethod]
        public CardAccountRequestModel[] GetListofDataWithSearch(string name, string fileNo, string licenseNo)
        {
            try
            {
                var result = new List<CardAccountRequestModel>();

                var cars = CardAccountRequestDL.GetListofDataWithSearch(name, fileNo, licenseNo);
                if(cars.Any())
                {
                    cars.ForEach(car =>
                    {
                        var c = new CardAccountRequestModel
                        {
                            ID = car.ID,
                            LicenseID = car.LicenseID,
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
                            PrintStatus = Convert.ToInt32(car.PrintStatus),
                            UserPrinting = car.UserPrinting
                        };

                        result.Add(c);
                    });
                }

                return result.ToArray();
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                throw ex;
            }
        }

        [WebMethod]
        public CardAccountRequestModel[] GetListofData()
        {
            try
            {
                var result = new List<CardAccountRequestModel>();

                var cars = CardAccountRequestDL.GetListofData();
                if (cars.Any())
                {
                    cars.ForEach(car =>
                    {
                        var c = new CardAccountRequestModel
                        {
                            ID = car.ID,
                            LicenseID = car.LicenseID,
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
                            PrintStatus = Convert.ToInt32(car.PrintStatus),
                            UserPrinting = car.UserPrinting
                        };

                        result.Add(c);
                    });
                }

                return result.ToArray();
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                throw ex;
            }
        }

        [WebMethod]
        public string UpdateStatus(string cardSerialNumber, string licenseNumber, int printStatus, string userPrinting, string printedName)
        {
            try
            {
                bool updateStatus = CardAccountRequestDL.UpdateStatus(cardSerialNumber, licenseNumber, printStatus, userPrinting, printedName);

                if(updateStatus)
                {
                    return "Success";
                }
                else
                {
                    return "Failed";
                }
            }
            catch(Exception ex)
            {
                ErrorHandler.WriteError(ex);
                throw ex;
            }
        }
    }
}
