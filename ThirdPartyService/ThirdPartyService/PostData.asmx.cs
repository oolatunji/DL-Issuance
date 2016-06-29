using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Services;
using ThirdPartyService.EntityFramework;

namespace ThirdPartyService
{
    /// <summary>
    /// Summary description for PostData
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PostData : System.Web.Services.WebService
    {

        [WebMethod]
        public Response SendDataForPrinting(PrintDataModel printData)
        {
            try
            {
                if (string.IsNullOrEmpty(printData.NameOnCard) || string.IsNullOrEmpty(printData.Pan) || string.IsNullOrEmpty(printData.UserPrinting) || string.IsNullOrEmpty(printData.UserPassword))
                {
                    return new Response
                    {
                        RecordID = 0,
                        Result = "Failed",
                        ErrorMessage = "Name on card, Pan, Username and Password Fields of the PrintDataModel are required."
                    };
                }
                else
                {
                    var user = ThirdPartyProcessor.AuthenticateUser(printData.UserPrinting, printData.UserPassword);
                    if (user == null)
                    {
                        return new Response
                        {
                            RecordID = 0,
                            Result = "Failed",
                            ErrorMessage = "Invalid Username/Password."
                        };
                    }
                    else
                    {
                        var recordID = 0;

                        var car = new CardAccountRequest
                        {
                            NameOnCard = printData.NameOnCard,
                            PAN = printData.Pan,
                            UserPrinting = printData.UserPrinting,
                            PrintStatus = 1,
                            DATE = System.DateTime.Now,
                            InstitutionName = printData.InstitutionName
                        };

                        var save = ThirdPartyProcessor.SaveCardAccountRequest(car, out recordID);

                        if (save)
                        {
                            return new Response
                            {
                                RecordID = recordID,
                                Result = "Success",
                                ErrorMessage = string.Empty
                            };
                        }
                        else
                        {
                            return new Response
                            {
                                RecordID = 0,
                                Result = "Failed",
                                ErrorMessage = "Operation Failed"
                            };
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                return new Response
                {
                    RecordID = 0,
                    Result = "Failed",
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
