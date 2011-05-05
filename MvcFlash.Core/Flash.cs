using System.Dynamic;
using MvcFlash.Core.Helpers;
using MvcFlash.Core.Providers;

namespace MvcFlash.Core
{
    public static class Flash
    {
        private const string FlashKey = "mvcflash.core.flash.container";
        private static readonly object _lock = new object();

        private static IFlashMessagePusher Pusher
        {
            get { return (IFlashMessagePusher)FlashConfiguration.Context.Items[FlashKey]; }
            set { FlashConfiguration.Context.Items[FlashKey] = value; }
        }

        static Flash() {
            Pusher = FlashConfiguration.Pusher;    
        }

        public static void Push(object message) {
            
            if(Pusher== null) {
                lock (_lock) {
                    if (Pusher == null) 
                        Pusher = FlashConfiguration.Pusher;
                }
            }
                
            Pusher.Push(message);
        }

        public static dynamic Success(string text, object context = null)
        {
            return Create(text, context, "success");
        }

        public static dynamic Notice(string text, object context = null)
        {
            return Create(text, context, "notice");
        }

        public static dynamic Warning(string text, object context = null)
        {
            return Create(text, context, "warning");
        }

        public static dynamic Error(string text, object context = null)
        {
            return Create(text, context, "error");
        }

        private static dynamic Create(string message, object context, string type)
        {
            dynamic flash = new ExpandoObject();

            flash.Text = message;
            flash.Context = DynamicHelpers.IfAnonymousCovertToExpando(context);
            flash.Type = type;

            Push(flash);
            return flash;
        }
    }
}
