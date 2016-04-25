using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseLibrary
{
    public class CardAccountRequestDTO
    {
        public long ID { get; set; }
        public string LicenseID { get; set; }
        public string CardSerialNumber { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        public string NameOnCard { get; set; }
        public string PrintedName { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public string MaritalStatus { get; set; }
        [Required]
        public string Sex { get; set; }
        [Required]
        public string Religion { get; set; }
        [Required]
        public string MothersMaidenName { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        public string UtilityBill { get; set; }
        [Required]
        public string IDNumber { get; set; }
        [Required]
        public string LocalGovernmentArea { get; set; }
        [Required]
        public string BloodGroup { get; set; }
        [Required]
        public string LicenseType { get; set; }
        [Required]
        public string IssueDate { get; set; }
        [Required]
        public string ValidTillDate { get; set; }
        [Required]
        public string FileNumber { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        public string PrintStatus { get; set; }
        public string UserPrinting { get; set; }
        public DateTime DateEnroled { get; set; }
        public DateTime DatePrinted { get; set; }
        public string Branch { get; set; }
    }
}
