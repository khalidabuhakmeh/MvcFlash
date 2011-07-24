using System.Web.Mvc;

namespace MvcFlash.Core.Filters
{
	/// <summary>
	/// Display a success message
	/// </summary>
	public class SuccessAttribute : ActionFilterAttribute {
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
		/// Initializes a new instance of the <see cref="SuccessAttribute"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public SuccessAttribute(string message)
		{
			Message = message;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext) {
			// probably don't need to do this, but hey just be safe
			if (string.IsNullOrWhiteSpace(Key)) {
				Flash.Success(Message);	
			} else {
				Flash.Unique(Key).Success(Message);
			}
		}
	}
}