using System;
using System.Web.Mvc;
using MvcFlash.Core;
using MvcFlash.Core.Filters;

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

        public ActionResult FlashSelect()
        {
            Flash.Error("oh no!");
            Flash.Warning("sucks");
            Flash.Error("something terrible again");

            Flash.Success("everything is fine and I live in a shell.");

            return View();
        }

        public ActionResult Custom()
        {
            Flash.Push(new {CrazyProperty = "this is a custom object"});

            return View();
        }

        public ActionResult Template()
        {
            return View();
        }

        public ActionResult TryRedirect()
        {
            Flash.Success("WooHoo");
            return RedirectToAction("Redirected");
        }

        public ActionResult Redirected()
        {
            return View();
        }

		public ActionResult Unique()
		{
			for (int i = 0; i < 10; i++) {
				Flash.Unique("success").Success("only display once");
			}

			return View();
		}

		[Notice("hey what's up?")]
		public ActionResult Attributes()
		{
			return View();
		}
    }
}
