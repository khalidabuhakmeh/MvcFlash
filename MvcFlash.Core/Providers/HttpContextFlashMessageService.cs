using System;
using System.Collections.Generic;
using System.Web;
using MvcFlash.Core.Helpers;

namespace MvcFlash.Core.Providers
{
    public class HttpContextFlashMessageService : IFlashMessageService
    {
        private readonly HttpContextBase _context;
        private const string MessagesKey = "mvcflash.core.httpcontext.messages";
        private readonly object _lock = new object();

        private List<object> Messages
        {
            get { return (List<object>)_context.Items[MessagesKey]; }
            set { _context.Items[MessagesKey] = value; }
        }

        public HttpContextFlashMessageService(HttpContextBase context = null)
        {
            try {
                _context = context ?? new HttpContextWrapper(HttpContext.Current);
            }
            catch (ArgumentNullException e) {
                throw new ArgumentException("you are not currently running where HttpContext is accessible", e);
            }
        }

        public void Push(object message)
        {
            lock (_lock) {
                Messages = Messages ?? new List<dynamic>();
                var converted = DynamicHelpers.IfAnonymousCovertToExpando(message);
                Messages.Add(converted);
            }
        }

        public ICollection<object> Pop()
        {
            lock(_lock) {
                Messages = Messages ?? new List<dynamic>();

                var temp = new List<dynamic>(Messages);
                Messages.Clear();
                return temp;
            }
        }

        public void Clear()
        {
            lock(_lock) {
                if (Messages != null)
                    Messages.Clear();
            }
        }
    }
}