using LicenseLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseLibrary
{
    public class CardPL
    {
        public static bool Save(List<string> cards, long branchID, out long successfulCards)
        {
            try
            {
                var result = false;

                var pregeneratedCards = new List<PregeneratedCard>();

                string key = System.Configuration.ConfigurationManager.AppSettings.Get("ekey");

                cards.ForEach(card =>
                {
                    string hashedCardNumber = PasswordHash.MD5Hash(card);

                    if(!CardDL.PregeneratedCardExists(hashedCardNumber))
                    {
                        var pregeneratedCard = new PregeneratedCard
                        {
                            CardNumber = Crypter.Encrypt(key, card),
                            HashedCardNumber = hashedCardNumber,
                            Status = false,
                            Branch = branchID,
                            DateUploaded = System.DateTime.Now
                        };
                        pregeneratedCards.Add(pregeneratedCard);
                    }
                });

                if(pregeneratedCards.Any())
                {
                    result = CardDL.Save(pregeneratedCards);
                }

                successfulCards = pregeneratedCards.Count();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CardDetails> RetrievePregeneratedCards()
        {
            try
            {
                string key = System.Configuration.ConfigurationManager.AppSettings.Get("ekey");

                var returnedPregeneratedCards = new List<CardDetails>();

                var pregeneratedCards = CardDL.RetrievePregeneratedCards();

                pregeneratedCards.ForEach(pregeneratedCard =>
                {
                    CardDetails pCard = new CardDetails
                    {
                        ID = pregeneratedCard.ID,
                        CardNumber = Crypter.Decrypt(key, pregeneratedCard.CardNumber),
                        Issued = Convert.ToBoolean(pregeneratedCard.Status),
                        Branch = pregeneratedCard.Branch1.Name,
                        BranchID = pregeneratedCard.Branch1.ID,
                        DateUploaded = Convert.ToDateTime(pregeneratedCard.DateUploaded).ToString("MM/dd/yyyy")
                    };

                    returnedPregeneratedCards.Add(pCard);
                });

                return returnedPregeneratedCards;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CardDetails> RetrieveAvailablePregeneratedCards()
        {
            try
            {
                string key = System.Configuration.ConfigurationManager.AppSettings.Get("ekey");

                var returnedPregeneratedCards = new List<CardDetails>();

                var pregeneratedCards = CardDL.RetrieveAvailablePregeneratedCards();

                pregeneratedCards.ForEach(pregeneratedCard =>
                {
                    CardDetails pCard = new CardDetails
                    {
                        ID = pregeneratedCard.ID,
                        CardNumber = Crypter.Decrypt(key, pregeneratedCard.CardNumber),
                        Issued = Convert.ToBoolean(pregeneratedCard.Status),
                        Branch = pregeneratedCard.Branch1.Name,
                        BranchID = pregeneratedCard.Branch1.ID,
                        DateUploaded = Convert.ToDateTime(pregeneratedCard.DateUploaded).ToString("MM/dd/yyyy")
                    };

                    returnedPregeneratedCards.Add(pCard);
                });

                return returnedPregeneratedCards;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CardDetails RetrievePregeneratedCard(string cardNumber)
        {
            try
            {
                string key = System.Configuration.ConfigurationManager.AppSettings.Get("ekey");

                var returnedPregeneratedCard = new CardDetails();

                var pregeneratedCard = CardDL.RetrievePregeneratedCard(cardNumber);

                if (pregeneratedCard != null)
                {
                    CardDetails pCard = new CardDetails
                        {
                            ID = pregeneratedCard.ID,
                            CardNumber = Crypter.Decrypt(key, pregeneratedCard.CardNumber),
                            Issued = Convert.ToBoolean(pregeneratedCard.Status),
                            Branch = pregeneratedCard.Branch1.Name,
                            BranchID = pregeneratedCard.Branch1.ID,
                            DateUploaded = Convert.ToDateTime(pregeneratedCard.DateUploaded).ToString("MM/dd/yyyy")
                        };

                    returnedPregeneratedCard = pCard;
                }
                else
                {
                    returnedPregeneratedCard = null;
                }

                return returnedPregeneratedCard;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
