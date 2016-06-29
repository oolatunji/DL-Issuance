using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThirdPartyService.EntityFramework;

namespace ThirdPartyService
{
    public class ThirdPartyProcessor
    {
        public static bool SaveCardAccountRequest(CardAccountRequest car, out int recordID)
        {
            try
            {
                using (var context = new CardIssuanceWEMAEntities())
                {
                    context.CardAccountRequests.Add(car);
                    context.SaveChanges();
                }
                recordID = car.id;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static User AuthenticateUser(string username, string password)
        {
            try
            {
                using (var context = new CardIssuanceWEMAEntities())
                {
                    var users = context.Users
                                        .Where(f => f.UserName == username && f.Password == password);

                    return users.Any() ? users.FirstOrDefault() : null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}