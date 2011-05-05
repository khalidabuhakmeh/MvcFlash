using System.Collections.Generic;
using System.Dynamic;
using System.Web.Mvc;

namespace MvcFlash.Core.Helpers
{
    public static class DynamicHelpers
    {
        public static dynamic IfAnonymousCovertToExpando(object anonymousObject)
        {
            // it's null, ok no need to convert this
            if (anonymousObject == null) return anonymousObject;

            // not compiled as internal, no need to convert this
            if (!anonymousObject.GetType().FullName.Contains("f__AnonymousType")) return anonymousObject;

            IDictionary<string, object> anonymousDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);
            return expando;
        }
    }
}
