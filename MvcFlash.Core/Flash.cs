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

		public static int Count() {
			return FlashConfiguration.Popper.Count();
		}

		public static void Push(object message) {
			Push(null, message);
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

		public static dynamic Success(string text, object context = null) {
			return Create(text, context, "success");
		}

		public static dynamic Notice(string text, object context = null) {
			return Create(text, context, "notice");
		}

		public static dynamic Warning(string text, object context = null) {
			return Create(text, context, "warning");
		}

		public static dynamic Error(string text, object context = null) {
			return Create(text, context, "error");
		}

		private static dynamic Create(string message, object context, string type, string key = null) {
			dynamic flash = new ExpandoObject();

			flash.Text = message;
			flash.Context = DynamicHelpers.IfAnonymousCovertToExpando(context);
			flash.Type = type;

			Push(key, flash);
			return flash;
		}

		public static IFlashUnique Unique(string key) {
			if (key == null)
				throw new ArgumentNullException("key");

			return new FlashUnique(key);
		}

		public interface IFlashUnique {
			void Push(object message);
			dynamic Success(string text, object context = null);
			dynamic Notice(string text, object context = null);
			dynamic Warning(string text, object context = null);
			dynamic Error(string text, object context = null);
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
