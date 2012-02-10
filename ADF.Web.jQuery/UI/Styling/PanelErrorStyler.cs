using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Adf.Core.Extensions;
using Adf.Web.Helper;
using Adf.Web.UI.Styling;

namespace Adf.Web.jQuery.UI.Styling
{
    public class PanelErrorStyler : IStyler
    {
        public void SetStyles(Control c)
        {
            Panel panel = c as Panel;

            if (panel != null)
            {
                panel.CssClass = (panel.CssClass +" adfj-record-invalid").Trim();

                IEnumerable<WebControl> webControls = ControlHelper.List<WebControl>(panel).Where(o => o.CssClass.Contains("adfj-record-item"));
                webControls.ForEach(o => o.CssClass = (o.CssClass + " ui-state-error").Trim());
            }
        }
    }
}
