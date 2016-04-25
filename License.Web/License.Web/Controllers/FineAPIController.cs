using LicenseLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace License.Web.Controllers
{
    public class FineAPIController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage RetrieveFines()
        {
            try
            {
                IEnumerable<FineDTO> fines = FinePL.RetrieveFines();
                object returnedfines = new { data = fines };
                return Request.CreateResponse(HttpStatusCode.OK, returnedfines);
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
