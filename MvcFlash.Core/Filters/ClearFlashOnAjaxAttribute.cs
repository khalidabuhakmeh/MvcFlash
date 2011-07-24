using System.Web.Mvc;

namespace MvcFlash.Core.Filters {
	/// <summary>
	/// If the request is AJAX, you probably are not rendering a view
	/// but instead returning Json or Xml, which probably also means
	/// you never called Html.Flash()
	/// </summary>
	public class ClearFlashOnAjaxAttribute : ActionFilterAttribute  {
		public override void OnResultExecuted(ResultExecutedContext filterContext) {
			if (filterContext.HttpContext.Request.IsAjaxRequest()) {
				FlashConfiguration.Popper.Clear();
			}
		}
	}
}
