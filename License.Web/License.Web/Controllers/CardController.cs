using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace License.Web.Controllers
{
    public class CardController : Controller
    {
        // GET: Card
        public ActionResult UploadPregeneratedCard()
        {
            return View();
        }

        public ActionResult ViewPregeneratedCard()
        {
            return View();
        }
    }
}