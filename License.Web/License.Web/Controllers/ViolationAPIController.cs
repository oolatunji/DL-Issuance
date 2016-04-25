using LicenseLibrary;
using LicenseLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace License.Web.Controllers
{
    public class ViolationAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveViolation([FromBody]Violation violation)
        {
            try
            {
                string errMsg = string.Empty;
                violation.Date = System.DateTime.Now;
                bool result = ViolationPL.Save(violation, out errMsg);
                if (string.IsNullOrEmpty(errMsg))
                    return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Violation added successfully.") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.BadRequest, errMsg);
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
        public HttpResponseMessage RetrieveViolations()
        {
            try
            {
                IEnumerable<ViolationDTO> violations = ViolationPL.RetrieveViolations();
                object returnedviolations = new { data = violations };
                return Request.CreateResponse(HttpStatusCode.OK, returnedviolations);
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
