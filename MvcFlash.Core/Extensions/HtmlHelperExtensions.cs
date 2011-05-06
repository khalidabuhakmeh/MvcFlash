using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcFlash.Core.Models;

namespace MvcFlash.Core.Extensions
{
    public static class HtmlHelperExtensions {

        /// <summary>
        /// Flashes all messages and calls Html.Partial on them with the template specified.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public static MvcHtmlString Flash(this HtmlHelper helper, string templateName = "Flash")
        {
            var popper = FlashConfiguration.Popper;
            if (popper == null) return MvcHtmlString.Empty;
            var messages = popper.Pop();

            return Create(messages, helper, templateName);
        }

        /// <summary>
        /// Flashes all messages and calls the template function passed.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateFunc">The template function.</param>
        /// <returns></returns>
        public static MvcHtmlString Flash(this HtmlHelper helper, Func<FlashContext, MvcHtmlString> templateFunc)
        {
            var messageBus = FlashConfiguration.Popper;
            if (messageBus == null) return MvcHtmlString.Empty;

            var messages = messageBus.Pop();
            var htmlBuilder = new StringBuilder(string.Empty);

            var count = 0;
            var total = messages.Count();
            foreach (var message in messages)
            {
                try {
                    var current = templateFunc.Invoke(new FlashContext { Message = message, Index = count++, Total = total });
                    htmlBuilder.AppendLine(current.ToHtmlString());
                }
                catch (Exception){
                }
            }

            return MvcHtmlString.Create(htmlBuilder.ToString());
        }

        /// <summary>
        /// Flashes the selected messages and keeps the remaining elements in the messages stack.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public static MvcHtmlString FlashSelect(this HtmlHelper helper, string filter, string templateName = "Flash")
        {
            var popper = FlashConfiguration.Popper;
            if (popper == null) return MvcHtmlString.Empty;
            var messages = popper.Select(x => x.Type == filter);

            return Create(messages, helper, templateName);
        }

        /// <summary>
        /// Flashes the selected messages and keeps the remaining elements in the messages stack.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public static MvcHtmlString FlashSelect(this HtmlHelper helper, Func<dynamic, bool> filter, string templateName = "Flash")
        {
            var popper = FlashConfiguration.Popper;
            if (popper == null) return MvcHtmlString.Empty;
            var messages = popper.Select(filter);

            return Create(messages, helper, templateName);
        }

        /// <summary>
        /// Flashes all messages and calls the template function passed.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="templateFunc">The template function.</param>
        /// <returns></returns>
        public static MvcHtmlString FlashSelect(this HtmlHelper helper, Func<dynamic, bool> filter, Func<FlashContext, MvcHtmlString> templateFunc)
        {
            var messageBus = FlashConfiguration.Popper;
            if (messageBus == null) return MvcHtmlString.Empty;

            var messages = messageBus.Select(filter);
            var htmlBuilder = new StringBuilder(string.Empty);

            var count = 0;
            var total = messages.Count();
            foreach (var message in messages)
            {
                try
                {
                    var current = templateFunc.Invoke(new FlashContext { Message = message, Index = count++, Total = total });
                    htmlBuilder.AppendLine(current.ToHtmlString());
                }
                catch (Exception)
                {
                }
            }

            return MvcHtmlString.Create(htmlBuilder.ToString());
        }

        /// <summary>
        /// Flashes only the messages that meet the type criteria, and disregards the rest of the messages.
        /// *** You will lose all messages that do not match the type ***
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="type">The type.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public static MvcHtmlString FlashOnly(this HtmlHelper helper, string type = null, string templateName = "Flash")
        {
            if (string.IsNullOrWhiteSpace(type)) return Flash(helper, templateName);

            var types = new[] {type};
            return FlashOnly(helper, x => Enumerable.Contains(types, x.Type), templateName);
        }

        /// <summary>
        /// Flashes only the messages that meet the type criteria, and disregards the rest of the messages.
        /// *** You will lose all messages that do not match the type ***
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="types">The types.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public static MvcHtmlString FlashOnly(this HtmlHelper helper, string[] types, string templateName = "Flash")
        {
            return FlashOnly(helper, x => Enumerable.Contains(types, x.Type), templateName);
        }


        /// <summary>
        /// Flashes only the messages that meet the type criteria, and disregards the rest of the messages.
        /// *** You will lose all messages that do not match the type ***
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="where">The where.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public static MvcHtmlString FlashOnly(this HtmlHelper helper, Func<dynamic,bool> where = null, string templateName = "Flash" )
        {
            var popper = FlashConfiguration.Popper;
            if (popper == null) return MvcHtmlString.Empty;

            var messages = where == null ? popper.Pop() : popper.Pop().Where(x =>
            {
                try {
                    return where.Invoke(x);
                }
                catch (Exception) {
                    return false;
                }
            });

            return Create(messages.ToList(), helper, templateName);
        }

        private static MvcHtmlString Create(ICollection<dynamic> messages, HtmlHelper helper, string templateName)
        {
            var templateFunc = new Func<FlashContext, MvcHtmlString>(ctx => helper.Partial(templateName, ctx, helper.ViewData));
            var htmlBuilder = new StringBuilder(string.Empty);

            var count = 0;
            var total = messages.Count;

            foreach (var message in messages)
            {
                var current = templateFunc.Invoke(new FlashContext { Message = message, Index = count++, Total = total });
                htmlBuilder.AppendLine(current.ToHtmlString());
            }

            return MvcHtmlString.Create(htmlBuilder.ToString());
        }
    }
}
