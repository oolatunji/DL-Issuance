using LicenseLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseLibrary
{
    public class CardDL
    {
        public static bool Save(List<PregeneratedCard> pregeneratedCards)
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    context.PregeneratedCards.AddRange(pregeneratedCards);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool PregeneratedCardExists(string pregeneratedCard)
        {
            try
            {
                var existingCard = new PregeneratedCard();
                using (var context = new LicenseDBEntities())
                {
                    existingCard = context.PregeneratedCards
                                    .Where(t => t.HashedCardNumber.Equals(pregeneratedCard))
                                    .FirstOrDefault();
                }

                if (existingCard == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PregeneratedCard RetrievePregeneratedCard(string pregeneratedCard)
        {
            try
            {
                var existingCard = new PregeneratedCard();
                using (var context = new LicenseDBEntities())
                {
                    existingCard = context.PregeneratedCards
                                    .Include("Branch1")
                                    .Where(t => t.HashedCardNumber.Equals(pregeneratedCard))
                                    .FirstOrDefault();
                }

                return existingCard;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PregeneratedCard GetCardSerialNumber()
        {
            try
            {
                var existingCard = new PregeneratedCard();
                using (var context = new LicenseDBEntities())
                {
                    existingCard = context.PregeneratedCards
                                    .Include("Branch1")
                                    .Where(t => t.Status == false)
                                    .Take(1)
                                    .FirstOrDefault();
                }

                return existingCard;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<PregeneratedCard> RetrievePregeneratedCards()
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    var pregeneratedCards = context.PregeneratedCards
                                .Include("Branch1")
                                .ToList();

                    return pregeneratedCards;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<PregeneratedCard> RetrieveAvailablePregeneratedCards()
        {
            try
            {
                using (var context = new LicenseDBEntities())
                {
                    var pregeneratedCards = context.PregeneratedCards
                                .Include("Branch1")
                                .Where(x => x.Status == false)
                                .ToList();

                    return pregeneratedCards;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
