using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcFlash.Core;

namespace MvcFlash.Sample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Flash.Success("WooHoo!");
            return View();
        }

        public ActionResult WithPartial()
        {
            Flash.Success("WooHoo!");
            return View();
        }

        public ActionResult FlashOnly()
        {
            Flash.Error("oh no!");
            Flash.Warning("sucks");
            Flash.Error("something terrible again");

            Flash.Success("everything is fine and I live in a shell.");

            return View();
        }

        public ActionResult FlashOnlyLambda()
        {
            Flash.Error("oh no!");
            Flash.Warning("sucks");
            Flash.Error("something terrible again");

            Flash.Success("everything is fine and I live in a shell.");

            return View();
        }

    }
}
