using System;
using System.Web.UI;
using ClientScriptManager = DNA.UI.ClientScriptManager;

namespace Adf.Web.jQuery
{
    internal static class jQuery
    {
        [Obsolete]
        public static void RegisterScripts(Page page, Control control)
        {
            ClientScriptManager.RegisterJQuery(control);
        }

        [Obsolete]
        public static void RegisterScript(Page page, params string[] js)
        {
            foreach (string j in js)
            {
//                ClientScriptManager.AddCompositeScript(page, typeof(jQuery).Namespace + "." + j, typeof(jQuery).Assembly.FullName);
                page.ClientScript.RegisterClientScriptResource(typeof(jQuery), typeof(jQuery).Namespace + "." + j);
            }
        }

        public static void RegisterCss(Page page, params string[] css)
        {
            foreach (string c in css)
            {
                string href = page.ClientScript.GetWebResourceUrl(typeof (jQuery), typeof (jQuery).Namespace + "." + c);
                page.Header.Controls.Add(ClientScriptManager.CreateStyleLink(href));
            }
        }
    }
}
