using System.Collections.Generic;
using System.Web;

namespace MvcFlash.Tests
{
	public class HttpSessionMock : HttpSessionStateBase {
		private readonly Dictionary<string, object> objects = new Dictionary<string, object>();

		public override object this[string name] {
			get { return (objects.ContainsKey(name)) ? objects[name] : null; }
			set { objects[name] = value; }
		}
	}
}