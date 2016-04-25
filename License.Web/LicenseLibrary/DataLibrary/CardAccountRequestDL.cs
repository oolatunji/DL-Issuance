﻿using LicenseLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseLibrary
{
    public class CardAccountRequestDL
    {
        public static bool Save(CardAccountRequest car, out long recordId)
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    context.CardAccountRequests.Add(car);
                    context.SaveChanges();
                }
                recordId = car.ID;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(CardAccountRequest car)
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    context.Entry(car).State = EntityState.Modified;
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateStatus(string cardSerialNumber, string licenseNumber, int printStatus, string userPrinting, string printedName)
        {
            try
            {
                bool result = false;

                using (var context = new LicenseDBEntities())
                {
                    using(var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var car = context.CardAccountRequests
                                    .Where(x => x.LicenseID == licenseNumber).FirstOrDefault();


                            string key = System.Configuration.ConfigurationManager.AppSettings.Get("ekey");
                            string hashedCardNumber = PasswordHash.MD5Hash(cardSerialNumber);

                            if (car != null)
                            {
                                car.PrintStatus = printStatus;
                                car.DatePrinted = System.DateTime.Now;
                                car.UserPrinting = userPrinting;
                                car.CardSerialNumber = Crypter.Encrypt(key, cardSerialNumber);
                                car.HashedCardSerialNumber = hashedCardNumber;
                                car.PrintedName = printedName;

                                context.Entry(car).State = EntityState.Modified;
                                context.SaveChanges();

                                var card = context.PregeneratedCards
                                    .Where(x => x.HashedCardNumber == hashedCardNumber).FirstOrDefault();

                                if (card != null)
                                {
                                    card.Status = true;

                                    context.Entry(card).State = EntityState.Modified;
                                    context.SaveChanges();

                                    result = true;
                                    transaction.Commit();
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static long RecordsToBePrinted(long branchID)
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    var cars = context.CardAccountRequests
                                    .Where(x => x.PrintStatus == 1 && x.BranchID == branchID)
                                    .ToList();

                    return cars.Any() ? cars.Count() : 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CardAccountRequest GetFullDataWithID(long ID)
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    var car = context.CardAccountRequests
                                    .Where(x => x.ID == ID).FirstOrDefault();

                    string key = System.Configuration.ConfigurationManager.AppSettings.Get("ekey");

                    if(car != null && !string.IsNullOrEmpty(car.CardSerialNumber))
                    {
                        car.CardSerialNumber = Crypter.Decrypt(key, car.CardSerialNumber);
                        car.HashedCardSerialNumber = Crypter.Decrypt(key, car.CardSerialNumber);
                    }

                    return car;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CardAccountRequest GetCardAccountRequestByLicenseID(string licenseID)
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    var car = context.CardAccountRequests
                                    .Where(x => x.LicenseID == licenseID).FirstOrDefault();

                    return car;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CardAccountRequest> GetListofDataWithSearch(string name, string fileNo, string licenseNo, long branchID)
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    var cars = context.CardAccountRequests
                                    .Where(x => (x.Lastname.Contains(name) 
                                        || x.FirstName.Contains(name) 
                                        || x.MiddleName.Contains(name)) && x.BranchID == branchID)
                                    .ToList();

                    if(!string.IsNullOrEmpty(fileNo))
                    {
                        cars = cars.Where(x => x.FileNumber == fileNo).ToList();
                    }

                    if (!string.IsNullOrEmpty(licenseNo))
                    {
                        cars = cars.Where(x => x.LicenseID == licenseNo).ToList();
                    }

                    return cars;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CardAccountRequest> GetListofData(long branchID)
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    var cars = context.CardAccountRequests                                    
                                    .Where(x => x.PrintStatus == 1 && x.BranchID == branchID)
                                    .ToList();

                    return cars;
                }
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

                using (var context = new LicenseDBEntities())
                {
                    var branchList = BranchDL.RetrieveBranches();
                    var cars = (from car in context.CardAccountRequests
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
                                    DateEnroled =  Convert.ToDateTime(car.DateEnroled),
                                    DatePrinted = Convert.ToDateTime(car.DatePrinted),
                                    Branch = branchList.Where(x => x.ID == car.BranchID).FirstOrDefault().Name
                                }).ToList();
                                    

                    return cars;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
