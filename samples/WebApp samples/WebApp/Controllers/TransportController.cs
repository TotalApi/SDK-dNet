using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Api;

namespace WebApp.Controllers
{
    public class TransportController : Controller
    {
        // GET: Transport
        public ActionResult Index()
        {
            ViewBag.Title = "Transport";
            return View();
        }
        // GET: Transport/Edit/
        public ActionResult Edit()
        {
            ViewBag.Title = "Edit Transport Item";
            return View("EditTransportItem");
        }
    }
}