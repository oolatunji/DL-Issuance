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
                return CardAccountRequestDL.RetrieveCardAccountRequests();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
