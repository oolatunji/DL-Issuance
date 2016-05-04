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
        public CardDetails GetCardSerialNumber(string loggedInUsername)
        {
            var pd = new CardDetails();
            try
            {
                if (!string.IsNullOrEmpty(loggedInUsername))
                {
                    var details = CardPL.GetCardSerialNumber(loggedInUsername);

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
                else
                {
                    pd.response = String.Format("{0}|{1}", "Failed", "Username of the logged in user is required.");
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
                if (!string.IsNullOrEmpty(enrolmentData.UserPrinting))
                {
                    var user = UserPL.RetrieveUserByUsername(enrolmentData.UserPrinting);
                    if(user == null)
                    {
                        throw new Exception("Invalid User Printing Username");
                    }
                    else if (user.ID == 0)
                    {
                        throw new Exception("Invalid User Printing Username");
                    }
                    else
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
                            PrintStatus = 1,
                            UserPrinting = enrolmentData.UserPrinting,
                            DateEnroled = System.DateTime.Now,
                            LicenseID = String.Format("{0:dMyyyyHHmmss}", System.DateTime.Now),
                            BranchID = user.UserBranch
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
                }
                else
                {
                    return new Response
                    {
                        Result = "Failed",
                        RecordID = 0,
                        ErrMessage = "User printing username is required."
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
                if (enrolmentData.ID != 0)
                {
                    var car = CardAccountRequestDL.RetrieveCardAccountRequestsByID(enrolmentData.ID);

                    car.Lastname = enrolmentData.Lastname;
                    car.FirstName = enrolmentData.FirstName;
                    car.MiddleName = enrolmentData.MiddleName;
                    car.NameOnCard = enrolmentData.NameOnCard;
                    car.DateOfBirth = enrolmentData.DateOfBirth;
                    car.MaritalStatus = enrolmentData.MaritalStatus;
                    car.Sex = enrolmentData.Sex;
                    car.Religion = enrolmentData.Religion;
                    car.MothersMaidenName = enrolmentData.MothersMaidenName;
                    car.Nationality = enrolmentData.Nationality;
                    car.UtilityBill = enrolmentData.UtilityBill;
                    car.IDNumber = enrolmentData.IDNumber;
                    car.LocalGovernmentArea = enrolmentData.LocalGovernmentArea;
                    car.BloodGroup = enrolmentData.BloodGroup;
                    car.LicenseType = enrolmentData.LicenseType;
                    car.IssueDate = enrolmentData.IssueDate;
                    car.ValidTillDate = enrolmentData.ValidTillDate;
                    car.FileNumber = enrolmentData.FileNumber;
                    car.EmailAddress = enrolmentData.EmailAddress;
                    car.PhoneNumber = enrolmentData.PhoneNumber;
                    car.Address = enrolmentData.Address;
                    car.Photo = enrolmentData.Photo;
                    car.FingerIdLeft = enrolmentData.FingerIdLeft;
                    car.FingerPrintLeft = enrolmentData.FingerPrintLeft;
                    car.FingerIdRight = enrolmentData.FingerIdRight;
                    car.FingerPrintRight = enrolmentData.FingerPrintRight;

                    bool saved = CardAccountRequestDL.Update(car);
                    if (saved)
                    {
                        return new Response
                        {
                            Result = "Success",
                            RecordID = car.ID,
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
                else
                {
                    return new Response
                    {
                        Result = "Failed",
                        RecordID = 0,
                        ErrMessage = "Enrolment Data ID is required"
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
        public long GetCountofRecords(string loggedInUsername)
        {
            try
            {
                var user = UserPL.RetrieveUserByUsername(loggedInUsername);
                if (user == null)
                {
                    throw new Exception("Invalid User Printing Username");
                }
                else if (user.ID == 0)
                {
                    throw new Exception("Invalid User Printing Username");
                }
                else
                {
                    return CardAccountRequestDL.RecordsToBePrinted(Convert.ToInt64(user.UserBranch));
                }
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
        public CardAccountRequestModel[] GetListofDataWithSearch(string name, string fileNo, string licenseNo, string loggedInUsername)
        {
            try
            {
                var user = UserPL.RetrieveUserByUsername(loggedInUsername);
                if (user == null)
                {
                    throw new Exception("Invalid User Printing Username");
                }
                else if (user.ID == 0)
                {
                    throw new Exception("Invalid User Printing Username");
                }
                else
                {
                    var result = new List<CardAccountRequestModel>();

                    var cars = CardAccountRequestDL.GetListofDataWithSearch(name, fileNo, licenseNo, Convert.ToInt64(user.UserBranch));
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
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                throw ex;
            }
        }

        [WebMethod]
        public CardAccountRequestModel[] GetListofData(string loggedInUsername)
        {
            try
            {
                var user = UserPL.RetrieveUserByUsername(loggedInUsername);
                if (user == null)
                {
                    throw new Exception("Invalid User Printing Username");
                }
                else if (user.ID == 0)
                {
                    throw new Exception("Invalid User Printing Username");
                }
                else
                {
                    var result = new List<CardAccountRequestModel>();

                    var cars = CardAccountRequestDL.GetListofData(Convert.ToInt64(user.UserBranch));
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
                if (string.IsNullOrEmpty(cardSerialNumber) || string.IsNullOrEmpty(licenseNumber) || printStatus == 0 || string.IsNullOrEmpty(userPrinting))
                {
                    throw new Exception("The fields CardSerialNumber, LicenseNumber, PrintStatus and UserPrinting are all required.");
                }
                else
                {
                    bool updateStatus = CardAccountRequestDL.UpdateStatus(cardSerialNumber, licenseNumber, printStatus, userPrinting, printedName);

                    if (updateStatus)
                    {
                        return "Success";
                    }
                    else
                    {
                        return "Failed";
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorHandler.WriteError(ex);
                throw ex;
            }
        }

        [WebMethod]
        public Response InsertFine(string licenseID, string details)
        {
            try
            {
                var fine = new Fine
                {
                    LicenseID = licenseID,
                    Details = details,
                    Date = System.DateTime.Now
                };

                var car = CardAccountRequestDL.GetCardAccountRequestByLicenseID(fine.LicenseID);

                if (car == null)
                {
                    return new Response
                    {
                        Result = "Failed",
                        RecordID = 0,
                        ErrMessage = string.Format("Invalid License ID: {0}", fine.LicenseID)
                    };
                }
                else if (car.ID == 0)
                {
                    return new Response
                    {
                        Result = "Failed",
                        RecordID = 0,
                        ErrMessage = string.Format("Invalid License ID: {0}", fine.LicenseID)
                    };
                }
                long fineID = 0;
                bool saved = FineDL.Save(fine, out fineID);
                if(saved)
                {
                    return new Response
                    {
                        Result = "Success",
                        RecordID = fineID,
                    };
                }
                else
                {
                    return new Response
                    {
                        Result = "Failed",
                        RecordID = 0,
                        ErrMessage = "Insert operation failed."
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    Result = "Failed",
                    RecordID = 0,
                    ErrMessage =  ex.Message
                };
            }
        }

        [WebMethod]
        public Fine[] RetrieveFines(string licenseID)
        {
            try
            {
                return FineDL.RetrieveFines(licenseID).ToArray();
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                throw ex;
            }
        }
    }
}
