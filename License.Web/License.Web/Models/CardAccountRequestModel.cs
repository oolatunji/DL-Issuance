﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.Web.Models
{
    public class CardAccountRequestModel
    {
        public long ID { get; set; }
        public string LicenseID { get; set; }
        public string CardSerialNumber { get; set; }
        public string Lastname { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string NameOnCard { get; set; }
        public string DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Sex { get; set; }
        public string Religion { get; set; }
        public string MothersMaidenName { get; set; }
        public string Nationality { get; set; }
        public string UtilityBill { get; set; }
        public string IDNumber { get; set; }
        public string LocalGovernmentArea { get; set; }
        public string BloodGroup { get; set; }
        public string LicenseType { get; set; }
        public string IssueDate { get; set; }
        public string ValidTillDate { get; set; }
        public string FileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string UserPrinting { get; set; }
        public int PrintStatus { get; set; }
    }
}