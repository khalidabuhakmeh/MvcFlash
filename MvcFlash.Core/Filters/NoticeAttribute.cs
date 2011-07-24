using System.Web.Mvc;

namespace MvcFlash.Core.Filters
{
	/// <summary>
	/// Display a notice
	/// </summary>
	public class NoticeAttribute : ActionFilterAttribute {
		/// <summary>
		/// Gets or sets the unique key, to prevent multiple showings of this particular notice.
		/// </summary>
		/// <value>The key.</value>
		public string Key { get; set; }

		/// <summary>
		/// The message to create a notice of
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="NoticeAttribute"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public NoticeAttribute(string message)
		{
			Message = message;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext) {
			// probably don't need to do this, but hey just be safe
			if (string.IsNullOrWhiteSpace(Key)) {
				Flash.Notice(Message);	
			} else {
				Flash.Unique(Key).Notice(Message);
			}
		}
	}
}