using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class BenchmarkController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Benchmark";

            return View();
        }
    }
}
