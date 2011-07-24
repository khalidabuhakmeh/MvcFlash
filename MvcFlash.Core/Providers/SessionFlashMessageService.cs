using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcFlash.Core.Helpers;

namespace MvcFlash.Core.Providers
{
    public class SessionFlashMessageService : IFlashMessageService
    {
        private readonly HttpContextBase _context;
        private const string MessagesKey = "mvcflash.core.session.messages";
        private readonly object _lock = new object();

        private IDictionary<string,object> Messages
        {
            get { return (IDictionary<string, object>) _context.Session[MessagesKey]; }
            set { _context.Session[MessagesKey] = value; }
        }

        public SessionFlashMessageService(HttpContextBase context = null)
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
    		Push(null, message);
    	}

    	public void Push(string key, object message)
        {
            lock (_lock)
            {
            	Messages = Messages ?? new ConcurrentDictionary<string, dynamic>();
                var converted = DynamicHelpers.IfAnonymousCovertToExpando(message);

				if (string.IsNullOrWhiteSpace(key)) {
					key = Guid.NewGuid().ToString();
				}
                
				// if key is specified, go ahead and overwrite the message
				if (Messages.ContainsKey(key)) {
					Messages[key] = converted;
				} else {
					Messages.Add(key, converted);	
				}
				
            }
        }

        public ICollection<dynamic> Pop()
        {
            lock (_lock)
            {
                Messages = Messages ?? new ConcurrentDictionary<string, object>();

                var temp = new ConcurrentDictionary<string, dynamic>(Messages);
                Messages.Clear();
                return temp.Values;
            }
        }

        public ICollection<dynamic> Select(Func<dynamic, bool> where)
        {
            lock (_lock)
            {
                Messages = Messages ?? new ConcurrentDictionary<string, object>();

                Func<dynamic, bool> filter = x =>
                {
                    try
                    {
                        return where.Invoke(x);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                };

                var temp = new List<dynamic>(Messages.Values.Where(filter).ToList());
            	var keysToRemove = Messages.Where(x => temp.Contains(x.Value)).Select(x => x.Key);

            	foreach (var removed in keysToRemove) {
            		Messages.Remove(removed);
            	}

                return temp;
            }
        }

    	public int Count()
    	{
    		return Messages.Count;
    	}

    	public void Clear()
        {
            lock (_lock)
            {
                if (Messages != null)
                    Messages.Clear();
            }
        }
    }
}
