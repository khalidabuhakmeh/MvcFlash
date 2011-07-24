using System.Web.Mvc;

namespace MvcFlash.Core.Filters
{
	/// <summary>
	/// So you need to clear flash on a particular action regardless
	/// of what happens. Flash messages are cleared after the result
	/// is done executing
	/// </summary>
	public class ClearFlashAttribute : ActionFilterAttribute {
		public override void OnResultExecuted(ResultExecutedContext filterContext) {
			FlashConfiguration.Popper.Clear();
		}
	}
}