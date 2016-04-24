using License.Web.Models;
using LicenseLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace License.Web.Controllers
{
    public class CardAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage UploadPregeneratedCards([FromBody]CardModel cardModel)
        {
            try
            {
                long successfulCards = 0;
                bool result = CardPL.Save(cardModel.CardNumbers.ToList(), cardModel.Branch, out successfulCards);
                if (successfulCards != 0)
                {
                    return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, string.Format("{0} unique pregenerated card(s) uploaded successfully.", successfulCards)) : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
                }
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.BadRequest, "Batch of card numbers uploaded already.");
                    return response;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage RetrievePregeneratedCards()
        {
            try
            {
                IEnumerable<CardDetails> pregeneratedCards = CardPL.RetrievePregeneratedCards();
                object returnedPregeneratedCards = new { data = pregeneratedCards };
                return Request.CreateResponse(HttpStatusCode.OK, returnedPregeneratedCards);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.ReasonPhrase = ex.Message;
                return response;
            }
        }
    }
}
