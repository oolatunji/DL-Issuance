using LicenseLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace License.Web.Controllers
{
    public class EnrolmentAPIController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage RetrieveEnrolments()
        {
            try
            {
                IEnumerable<CardAccountRequestDTO> cars = CardAccountRequestPL.RetrieveCardAccountRequests();
                object returnedcars = new { data = cars };
                return Request.CreateResponse(HttpStatusCode.OK, returnedcars);
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
