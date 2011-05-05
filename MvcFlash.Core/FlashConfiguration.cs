using System;
using System.Web;
using MvcFlash.Core.Providers;

namespace MvcFlash.Core
{
    public static class FlashConfiguration
    {
        static Func<IFlashMessageService> _serviceMaker = () => new HttpContextFlashMessageService();
        private static Func<HttpContextBase> _contextMaker = () => new HttpContextWrapper(HttpContext.Current);

        public static void WithService(Func<IFlashMessageService> config) {
            _serviceMaker = config ?? _serviceMaker;
        }

        public static void WithContext(Func<HttpContextBase> context)
        {
            _contextMaker = context ?? _contextMaker;
        }

        internal static IFlashMessagePusher Pusher {
            get { return _serviceMaker.Invoke(); }
        }

        internal static IFlashMessagePopper Popper {
            get { return _serviceMaker.Invoke(); }
        }

        internal static HttpContextBase Context {
            get { return _contextMaker.Invoke(); }
        }
    }
}
