using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace License.Web.Controllers
{
    public class SystemController : Controller
    {
        // GET: System
        public ActionResult UnAuthorized()
        {
            return View();
        }

        public ActionResult NotFoundError()
        {
            return View();
        }

        public ActionResult InternalServerError()
        {
            return View();
        }
    }
}