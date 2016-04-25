using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace License.Web.Controllers
{
    public class ViolationController : Controller
    {
        // GET: Violation
        public ActionResult AddViolation()
        {
            return View();
        }

        public ActionResult ViewViolations()
        {
            return View();
        }
    }
}