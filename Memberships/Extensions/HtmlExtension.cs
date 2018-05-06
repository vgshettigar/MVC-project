using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Memberships.Extensions
{
    public static class HtmlExtension
    {
        public static MvcHtmlString GlyphLink(this HtmlHelper htmlhelper, 
            string controller, string action, string text, string glyphicon, string cssClasses="",
            string id="", Dictionary<string, string> attributes=null)
        {
            //declare a span for glyphIcon
            var glyph = string.Format("<span class='glyphicon glyphicon-{0}'></span>", glyphicon);

            var anchor = new TagBuilder("a");
            if (controller.Length > 0)
                anchor.MergeAttribute("href", string.Format("/{0}/{1}/", controller, action));
            else
                anchor.MergeAttribute("href", "#");

            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    anchor.MergeAttribute(attribute.Key, attribute.Value);

                }
            }
           anchor.InnerHtml = string.Format("{0} {1}", glyph, text);

            anchor.AddCssClass(cssClasses);
            anchor.GenerateId(id);

            return MvcHtmlString.Create(anchor.ToString(TagRenderMode.Normal));

        }
    }
}