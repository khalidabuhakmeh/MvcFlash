using System;
using System.Dynamic;
using MvcFlash.Core.Helpers;
using MvcFlash.Core.Providers;

namespace MvcFlash.Core {
	public static class Flash {
		private const string FlashKey = "mvcflash.core.flash.container";
		private static readonly object _lock = new object();

		private static IFlashMessagePusher Pusher {
			get { return (IFlashMessagePusher)FlashConfiguration.Context.Items[FlashKey]; }
			set { FlashConfiguration.Context.Items[FlashKey] = value; }
		}

		static Flash() {
			Pusher = FlashConfiguration.Pusher;
		}

		/// <summary>
		/// Return the current count of messages found in Flash.
		/// </summary>
		/// <returns></returns>
		public static int Count {
			get { return FlashConfiguration.Popper.Count(); }
		}

		/// <summary>
		/// Pushes your own custom message into Flash.
		/// </summary>
		/// <param name="message">The message.</param>
		public static void Push(object message) {
			Push(null, message);
		}

		/// <summary>
		/// Pushes a success message into Flash.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public static dynamic Success(string text, object context = null) {
			return Create(text, context, "success");
		}

		/// <summary>
		/// Pushes a notice message into Flash.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public static dynamic Notice(string text, object context = null) {
			return Create(text, context, "notice");
		}

		/// <summary>
		/// Pushes a warning message into Flash.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public static dynamic Warning(string text, object context = null) {
			return Create(text, context, "warning");
		}

		/// <summary>
		/// Pushes and error message into Flash.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public static dynamic Error(string text, object context = null) {
			return Create(text, context, "error");
		}

		/// <summary>
		/// Makes sure that this message only ever comes up once, no matter how many times it gets pushed.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public static IFlashUnique Unique(string key) {
			if (key == null)
				throw new ArgumentNullException("key");

			return new FlashUnique(key);
		}

		/// <summary>
		/// Unique's available messages 
		/// </summary>
		public interface IFlashUnique {
			void Push(object message);
			dynamic Success(string text, object context = null);
			dynamic Notice(string text, object context = null);
			dynamic Warning(string text, object context = null);
			dynamic Error(string text, object context = null);
		}

		private static dynamic Create(string message, object context, string type, string key = null) {
			dynamic flash = new ExpandoObject();

			flash.Text = message;
			flash.Context = DynamicHelpers.IfAnonymousCovertToExpando(context);
			flash.Type = type;

			Push(key, flash);
			return flash;
		}

		private static void Push(string key, object message) {
			if (Pusher == null) {
				lock (_lock) {
					if (Pusher == null)
						Pusher = FlashConfiguration.Pusher;
				}
			}

			Pusher.Push(key,message);
		}

		internal class FlashUnique : IFlashUnique {
			private readonly string _key;

			public FlashUnique(string key) {
				_key = key;
			}

			public void Push(object message) {
				Flash.Push(_key, message);	
			}

			public dynamic Success(string text, object context = null) {
				return Flash.Create(text, context, "success", _key);
			}

			public dynamic Notice(string text, object context = null) {
				return Flash.Create(text, context, "notice", _key);
			}

			public dynamic Warning(string text, object context = null) {
				return Flash.Create(text, context, "warning", _key);
			}

			public dynamic Error(string text, object context = null) {
				return Flash.Create(text, context, "error", _key);
			}
		}
	}
}
