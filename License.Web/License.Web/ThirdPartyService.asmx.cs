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
        public CardDetails[] RetrieveAvailableCards()
        {
            try
            {
                return CardPL.RetrieveAvailablePregeneratedCards().ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
                ErrorHandler.WriteError(ex);
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
    }
}
