﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace License.Web.Controllers
{
    public class FineController : Controller
    {
        // GET: Fine
        public ActionResult ViewFines()
        {
            return View();
        }
    }
}